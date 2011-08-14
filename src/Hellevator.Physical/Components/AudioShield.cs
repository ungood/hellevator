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

        protected SpiCoordinator Coordinator { get; private set; }
        protected BufferSpiWriter CommandWriter { get; private set; }
        protected StreamSpiWriter DataWriter { get; private set; }
        

        public AudioShield(SpiCoordinator coordinator, Cpu.Pin dataSelectPin, Cpu.Pin cmdSelectPin, Cpu.Pin dreqPin)
        {
            Coordinator = coordinator;

            var dreq = new InputPort(dreqPin, false, Port.ResistorMode.PullUp);
            
            var cmdConfig = new SPI.Configuration(cmdSelectPin, false, 0, 0, false, true, 2000, coordinator.Module);
            CommandWriter = new BufferSpiWriter(cmdConfig, dreq);

            var dataConfig = new SPI.Configuration(dataSelectPin, false, 0, 0, false, true, 2000, coordinator.Module);
            DataWriter = new StreamSpiWriter(dataConfig, dreq);

            Coordinator.Add(CommandWriter);
            Coordinator.Add(DataWriter);
        }

        public void Initialize()
        {
            if(!Coordinator.IsInitialized)
                throw new InvalidOperationException("Initialize SpiCoordinator before initialize AudioShields");

            Reset();

            WriteMode(Mode.SdiNew);
            WriteRegister(Register.ClockFreq, ClockFreq);
            SetVolume(255, 255);
        }

        private readonly byte[] cmdBuffer = new byte[4];

        /// <summary>
        /// Writes 16bit value to a register
        /// </summary>
        protected void WriteRegister(Register register, ushort data)
        {
            CommandWriter.Write(
                (byte) Direction.Write,
                (byte) register,
                (byte) (data >> 8),
                (byte) data);
            CommandWriter.BusyEvent.WaitOne();
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

        public void Play(Stream stream)
        {
            DataWriter.Play(stream);
        }
    }
}