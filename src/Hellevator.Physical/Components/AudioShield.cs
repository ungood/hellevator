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
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    public class AudioShield
    {
        #region Constants

        private const ushort ClockFreq = 0xa000;
        protected const int BlockSize = 32;

        public enum Direction : byte
        {
            Write = 0x02,
            Read  = 0x03,
        }

        [Flags]
        public enum Mode : ushort
        {
            Reset  = 0x0004,
            Cancel = 0x0010,
            Tests  = 0x0020,
            SdiNew = 0x0800,
            ADPCM  = 0x1000,
            Line1  = 0x4000,
        }

        public enum Register : byte
        {
            /// <summary>
            /// R/W Mode control
            /// </summary>
            Mode      = 0x00,

            /// <summary>
            /// R/W Status of VS1053b
            /// </summary>
            Status    = 0x01,
            
            /// <summary>
            /// R/W Bass/Treble control
            /// </summary>
            Bass      = 0x02,

            /// <summary>
            /// R/W Clock Frequency + Multiplier
            /// </summary>
            ClockFreq = 0x03,

            /// <summary>
            /// R/W Volume control
            /// </summary>
            Volume    = 0x0B,
            
        }

        #endregion

        protected readonly SpiCoordinator Coordinator;
        protected readonly ManualResetEvent DreqWait = new ManualResetEvent(false);
        protected readonly SPI.Configuration DataConfig;
        protected readonly SPI.Configuration CmdConfig;
        private readonly InputPort dreq;
        
        public AudioShield(SpiCoordinator coordinator, Cpu.Pin dataSelectPin, Cpu.Pin cmdSelectPin, Cpu.Pin dreqPin)
        {
            Coordinator = coordinator;
            DataConfig = new SPI.Configuration(dataSelectPin, false, 0, 0, false, true, 2000, coordinator.Module, dreqPin, false);
            CmdConfig = new SPI.Configuration(cmdSelectPin, false, 0, 0, false, true, 2000, coordinator.Module, dreqPin, false);
            dreq = new InputPort(dreqPin, false, Port.ResistorMode.PullUp);

            Initialize();
        }

        protected void WaitForDreq()
        {
            while(dreq.Read() == false)
                Thread.Sleep(10);
        }

        public void Initialize()
        {
            Reset();

            WriteMode(Mode.SdiNew);
            WriteRegister(Register.ClockFreq, ClockFreq);
            
            // Test if initialized
            WriteRegister(Register.Volume, 0x0101);
            if(ReadRegister(Register.Volume) != (0x0101))
            {
                // TODO: throw new Exception("Failed to initialize MP3 Decoder.");
            }
        }

        private readonly byte[] cmdBuffer = new byte[4];

        /// <summary>
        /// Reads 16bit value from a register
        /// </summary>
        protected ushort ReadRegister(Register register)
        {
            WaitForDreq();

            cmdBuffer[0] = (byte) Direction.Read;
            cmdBuffer[1] = (byte) register;
            cmdBuffer[2] = 0;
            cmdBuffer[3] = 0;
            Coordinator.Execute(CmdConfig, spi => spi.WriteRead(cmdBuffer, cmdBuffer, 2));

            return (ushort) (cmdBuffer[0] << 8 | cmdBuffer[1]);
        }

        /// <summary>
        /// Writes 16bit value to a register
        /// </summary>
        protected void WriteRegister(Register register, ushort data)
        {
            WaitForDreq();

            cmdBuffer[0] = (byte) Direction.Write;
            cmdBuffer[1] = (byte) register;
            cmdBuffer[2] = (byte) (data >> 8);
            cmdBuffer[3] = (byte) data;
            Coordinator.Execute(CmdConfig, spi => spi.Write(cmdBuffer));
        }

        protected void WriteMode(Mode mode)
        {
            WriteRegister(Register.Mode, (ushort) mode);
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

        public void SineTest()
        {
            WriteMode(Mode.SdiNew | Mode.Tests | Mode.Reset);

            var start = new byte[] {0x53, 0xEF, 0x6E, 0x7E};
            var zero = new byte[] {0x00, 0x00, 0x00, 0x00};
            var end = new byte[] {0x45, 0x78, 0x69, 0x74};
        
            Coordinator.Execute(DataConfig, spi => spi.Write(start));
            Coordinator.Execute(DataConfig, spi => spi.Write(zero));
            Thread.Sleep(2000);
            Coordinator.Execute(DataConfig, spi => spi.Write(end));
            Coordinator.Execute(DataConfig, spi => spi.Write(zero));
        }
    }
}