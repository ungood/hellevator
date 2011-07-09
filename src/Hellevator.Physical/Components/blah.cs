using System;
using Microsoft.SPOT;
using System.Threading;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.FEZ;

namespace shiftRegister
{
    // Shift register interface class
    // For use with FEZDomino, probably works with FEZMini and FEZCobra too, maybe others
    //
    // v 0.2 11/07/2010
    //
    // By Slugsie
    //
    // Licensed under GPLv3
    // Basically you are free to do what you want with this code.
    // You are encouraged to share any changes/improvements/bug fixes
    // back with the community.
    //
    // Warranty - None, neither explicit nor implied. Use at your own risk!
    //
    // Future enhancements:
    //  1. Implement TLC5940 shift register. This is a nifty 16 pin shift register with 12bit PWM
    //
    public class HC595
    {
        // 74HC595 shift register interface class
        //
        // How a 74HC595 shift register works:
        //  Basic operation of a 74HC595 is to start off by setting a latch pin low
        //  Set the clock low, put a bit on the data bus, set the clock high
        //  Repeat for all 8 bits (single 74HC595)
        //  Then set the latch pin high to write the data to the output pins
        //
        // Future enhancements:
        //  1. Implement multiple registers attached in series
        //  2. Implement driving of a 7 segment numeric LED display
        //
        #region Private variables
        private OutputPort m_latchPin;         // Latch pin, used to 'Latch' the data to the output pins on the register
        private OutputPort m_dataPin;          // Data pin, used to send the data to the register
        private OutputPort m_clockPin;         // Clock pin, used to signal the data to the register
        private Boolean m_bLSBFirst = false;   // Are we sending the LSB first, or the MSB?
        private int m_intNumRegisters = 1;     // Number of registers attached in series. Not yet implemented so weird things will happen if this is anything other than 1
        private byte[] m_bytCurrentState;      // Holds the current state of each shift register
        private byte[] m_bytBitMask = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 }; // Simple bit mask used to address each bit
        #endregion
        /// <remarks>
        /// Creates new instance of a 74HC595 shift register.
        /// Requires : 
        /// Pin used for data, 
        /// Pin used for clock, 
        /// Pin used for latch, 
        /// Number of shift registers, 
        /// MSB first or last
        /// </remarks>
        #region Interface
        /// <summary>
        /// Creates new instance of a 74HC595 shift register
        /// </summary>
        /// <param name="dataPin">Pin used for data pulse</param>
        /// <param name="clockPin">Pin used for clock pulse</param>
        /// <param name="latchPin">Pin used for latch pulse</param>
        public HC595(Cpu.Pin dataPin, Cpu.Pin clockPin, Cpu.Pin latchPin)
        {
            Setup(dataPin, clockPin, latchPin, 1, false);
        }
        /// <summary>
        /// Creates new instance of a 74HC595 shift register
        /// </summary>
        /// <param name="dataPin">Pin used for data pulse</param>
        /// <param name="clockPin">Pin used for clock pulse</param>
        /// <param name="latchPin">Pin used for latch pulse</param>
        /// <param name="bigEndian">Specify which end of the byte to send in first</param>
        public HC595(Cpu.Pin dataPin, Cpu.Pin clockPin, Cpu.Pin latchPin, Boolean lsbFirst)
        {
            Setup(dataPin, clockPin, latchPin, 1, lsbFirst);
        }
        /// <summary>
        /// Creates new instance of a 74HC595 shift register
        /// </summary>
        /// <param name="dataPin">Pin used for data pulse</param>
        /// <param name="clockPin">Pin used for clock pulse</param>
        /// <param name="latchPin">Pin used for latch pulse</param>
        /// <param name="numRegisters">Number of shift registers attached in series</param>
        public HC595(Cpu.Pin dataPin, Cpu.Pin clockPin, Cpu.Pin latchPin, int numRegisters)
        {
            Setup(dataPin, clockPin, latchPin, numRegisters, false);
        }
        /// <summary>
        /// Creates new instance of a 74HC595 shift register
        /// </summary>
        /// <param name="dataPin">Pin used for data pulse</param>
        /// <param name="clockPin">Pin used for clock pulse</param>
        /// <param name="latchPin">Pin used for latch pulse</param>
        /// <param name="numRegisters">Number of shift registers attached in series</param>
        /// <param name="bigEndian">Specify which end of the byte to send in first</param>
        public HC595(Cpu.Pin dataPin, Cpu.Pin clockPin, Cpu.Pin latchPin, int numRegisters, Boolean lsbFirst)
        {
            Setup(dataPin, clockPin, latchPin, numRegisters, lsbFirst);
        }
        // This private method is used by the various public methods to actually do the setup work
        private void Setup(Cpu.Pin dataPin, Cpu.Pin clockPin, Cpu.Pin latchPin, int numRegisters, Boolean lsbFirst)
        {
            // Define the pins
            m_dataPin = new OutputPort(dataPin, false);
            m_clockPin = new OutputPort(clockPin, false);
            m_latchPin = new OutputPort(latchPin, false);
            // Which end of the data gets sent first
            m_bLSBFirst = lsbFirst;
            // How many registers are we working with
            m_intNumRegisters = numRegisters;
            // Set the current state, and send it to the register
            m_bytCurrentState = new byte[m_intNumRegisters];
            for (int iLoop = 0; iLoop < m_intNumRegisters; iLoop++)
            {
                m_bytCurrentState[iLoop] = 0;
            }
            SendByte(m_bytCurrentState[0]);
        }
        #endregion

        /// <summary>
        /// Used to set/unset an individual bit on the register
        /// </summary>
        /// <param name="pinNumber">Which pin number (0-7)</param>
        /// <param name="state">Set pin high or low (True/False)</param>
        public void SetBit(int pinNumber, Boolean state)
        {
            // Pass on to the SetBit that accepts a registerNumber, and default it to register 0
            SetBit(pinNumber, 0, state);
        }

        /// <summary>
        /// Used to set/unset an individual bit on the register
        /// </summary>
        /// <param name="pinNumber">Which pin number (0-7)</param>
        /// <param name="registerNumber">Which shift register to use in a serial chain</param>
        /// <param name="state">Set pin high or low (True/False)</param>
        public void SetBit(int pinNumber, int registerNumber, Boolean state)
        {
            if (state)
            { // We're setting the bit to high (True)
                m_bytCurrentState[registerNumber] = (byte)(m_bytCurrentState[registerNumber] | m_bytBitMask[pinNumber]);
            }
            else
            { // We're setting the bit to low (False)
                m_bytCurrentState[registerNumber] = (byte)(m_bytCurrentState[registerNumber] & ~m_bytBitMask[pinNumber]);
            }
            // Send the new state out
            SendByte(m_bytCurrentState[0]);
        }

        /// <summary>
        /// Send out one byte to shift register
        /// </summary>
        /// <param name="data">The byte of data to send</param>
        public void SendByte(byte data)
        {
            // Set latch low, send data, the set latch high to write the output ports
            m_latchPin.Write(false);
            ShiftOut(data);
            m_latchPin.Write(true);
        }

        /// <summary>
        /// Send out an array of bytes to shift register
        /// </summary>
        /// <param name="data">The array of byte data to send</param>
        public void SendByte(byte[] data)
        {
            // Set latch low, send each byte from the array, then set the latch high to write the output ports
            m_latchPin.Write(false);
            foreach (byte bytData in data)
            {
                ShiftOut(bytData);
            }
            m_latchPin.Write(true);
        }

        /// <summary>
        /// Provides a quick way to shift a single bit of data to the register.
        /// </summary>
        /// <param name="data">The bit to be sent</param>
        public void SendBit(Boolean data)
        {
            // Set the latch low, shift the data, pulse the clock, and set the latch high
            m_latchPin.Write(false);
            m_dataPin.Write(data);
            m_clockPin.Write(true);
            m_clockPin.Write(false);
            m_latchPin.Write(true);
        }
        
        /// <summary>
        /// Send out one byte in serial transfer
        /// </summary>
        /// <param name="data">The byte of data to be sent out</param>
        private void ShiftOut(byte data)
        {
            int start = 7;
            int end = 0;
            int incr = -1;

            if (m_bLSBFirst)
            {
                start = 0;
                end = 7;
                incr = 1;
            }

            for (int step = start; step != end + incr; step += incr)
            {
                // Check to see if the bit is set, and send the result to the register
                byte output = (byte)(data & m_bytBitMask[step]);
                m_dataPin.Write((output != 0));
                // Pulse clock port
                m_clockPin.Write(true);
                m_clockPin.Write(false);
            }

            // Save the current state - used for setting individual bits
            m_bytCurrentState[0] = data;
        }
    }
}
