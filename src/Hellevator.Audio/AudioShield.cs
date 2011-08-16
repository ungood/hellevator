#region License
// Copyright 2011 Jason Walker
// ungood@onetrue.name
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

using System;
using System.IO;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Audio
{
    public class AudioShield
    {
        #region Constants

        private const ushort ClockFreq = 0xa000;
        protected const int BlockSize = 32;

        protected enum Direction : byte
        {
            Write = 0x02,
            Read = 0x03,
        }

        [Flags]
        protected enum Mode : ushort
        {
            Reset = 0x0004,
            Cancel = 0x0010,
            Tests = 0x0020,
            SdiNew = 0x0800,
            ADPCM = 0x1000,
            Line1 = 0x4000,
        }

        protected enum Register : byte
        {
            /// <summary>
            /// R/W Mode control
            /// </summary>
            Mode = 0x00,

            /// <summary>
            /// R/W Status of VS1053b
            /// </summary>
            Status = 0x01,

            /// <summary>
            /// R/W Bass/Treble control
            /// </summary>
            Bass = 0x02,

            /// <summary>
            /// R/W Clock Frequency + Multiplier
            /// </summary>
            ClockFreq = 0x03,

            /// <summary>
            /// Read/write value from RAM.
            /// </summary>
            RamValue = 0x06,

            /// <summary>
            /// Sets address to read/write from RAM.
            /// </summary>
            RamAddress = 0x07,

            /// <summary>
            /// R/W Volume control
            /// </summary>
            Volume = 0x0B,
        }

        protected enum RamAddress : ushort
        {
            EndFillByte = 0x1e06
        };
        
        #endregion

        private readonly SPI.Configuration dataConfig;
        private readonly SPI.Configuration cmdConfig;
        private readonly InputPort dreq;
        private readonly SPI spi;
        private readonly object spiLock = new object();

        public AudioShield(SPI.SPI_module module, Cpu.Pin dataSelectPin, Cpu.Pin cmdSelectPin, Cpu.Pin dreqPin)
        {
            dataConfig = new SPI.Configuration(dataSelectPin, false, 0, 0, false, true, 2000, module, dreqPin, false);
            cmdConfig = new SPI.Configuration(cmdSelectPin, false, 0, 0, false, true, 2000, module, dreqPin, false);
            dreq = new InputPort(dreqPin, false, Port.ResistorMode.PullUp);
            spi = new SPI(cmdConfig);
        }

        public void Initialize()
        {
            Reset();

            WriteMode(Mode.SdiNew);
            WriteRegister(Register.ClockFreq, ClockFreq);
            SetVolume(255, 255);
        }

        protected void WaitForDreq()
        {
            while(dreq.Read() == false)
                Thread.Sleep(1);
        }

        protected void WriteCommand(ref byte[] buffer)
        {
            WaitForDreq();

            lock(spiLock)
            {
                spi.Config = cmdConfig;
                spi.WriteRead(buffer, buffer);
            }
        }

        protected void WriteData(byte[] buffer)
        {
            WaitForDreq();

            lock(spiLock)
            {
                spi.Config = dataConfig;
                spi.Write(buffer);
            }
        }

        /// <summary>
        /// Reads 16bit value from a register
        /// </summary>
        protected ushort ReadRegister(Register register)
        {
            WaitForDreq();

            var buffer = new byte[4];
            buffer[0] = (byte) Direction.Read;
            buffer[1] = (byte) register;
            buffer[2] = 0;
            buffer[3] = 0;

            WriteCommand(ref buffer);
            return (ushort) (buffer[0] << 8 | buffer[1]);
        }

        /// <summary>
        /// Writes 16bit value to a register
        /// </summary>
        protected void WriteRegister(Register register, ushort data)
        {
            WaitForDreq();

            var buffer = new byte[4];
            buffer[0] = (byte) Direction.Write;
            buffer[1] = (byte) register;
            buffer[2] = (byte) (data >> 8);
            buffer[3] = (byte) data;

            WriteCommand(ref buffer);
        }

        protected void WriteMode(Mode mode)
        {
            WriteRegister(Register.Mode, (ushort) mode);
        }

        protected ushort ReadAddress(RamAddress address)
        {
            WriteRegister(Register.RamAddress, (ushort) address);
            return ReadRegister(Register.RamValue);
        }

        /// <summary>
        /// Performs soft reset
        /// </summary>
        protected void Reset()
        {
            WriteMode(Mode.SdiNew | Mode.Reset);
            Thread.Sleep(1);

            WriteRegister(Register.ClockFreq, ClockFreq);
            WaitForDreq();
        }

        /// <summary>
        /// Set volume for both channels. Valid values 0-255
        /// </summary>
        /// <param name="leftChannelVolume">0 - silence, 255 - loudest</param>
        /// <param name="rightChannelVolume">0 - silence, 255 - loudest</param>
        public void SetVolume(byte leftChannelVolume, byte rightChannelVolume)
        {
            WriteRegister(Register.Volume, (ushort) ((255 - leftChannelVolume) << 8 | (255 - rightChannelVolume)));
        }

        
    }
}