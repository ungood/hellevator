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

using System.IO;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Audio
{
    public delegate void PlayFinishedEventHandler();

    public class AudioShieldPlayer : AudioShield
    {
        public AudioShieldPlayer(SPI.SPI_module module, Cpu.Pin dataSelectPin, Cpu.Pin cmdSelectPin, Cpu.Pin dreqPin)
            : base(module, dataSelectPin, cmdSelectPin, dreqPin) {}

        private Thread playThread;
        private Stream playStream;
        private bool stopRequested;

        public bool IsPlaying { get; private set; }
        public event PlayFinishedEventHandler PlayFinished;

        protected void OnPlayFinished()
        {
            IsPlaying = false;

            var handler = PlayFinished;
            if(handler != null)
                handler();
        }

        public void Play(Stream stream)
        {
            if(IsPlaying)
                Stop();

            SetVolume(255, 255);
            stopRequested = false;
            playStream = stream;
            playThread = new Thread(PlayStreamTask) {
                Priority = ThreadPriority.Highest
            };
            playThread.Start();
        }

        private void PlayStreamTask()
        {
            var block = new byte[BlockSize];
            IsPlaying = true;

            while(true)
            {
                if(stopRequested)
                {
                    FinishPlaying(true);
                    return;
                }

                var bytesRead = playStream.Read(block, 0, BlockSize);
                if(bytesRead < 1)
                    break;

                WriteData(block);
            }

            FinishPlaying(false);
        }

        private void FinishPlaying(bool cancelled)
        {
            var endFillByte = ReadAddress(RamAddress.EndFillByte);
            var buffer = new byte[BlockSize];
            for(var i = 0; i < BlockSize; i++)
                buffer[i] = (byte)endFillByte;

            if(!cancelled)
            {
                for(var i = 0; i <= 2052 / BlockSize; i++)
                    WriteData(buffer);
            }

            WriteMode(Mode.Cancel | Mode.SdiNew);

            var repeat = 0;
            while(true)
            {
                WriteData(buffer);
                var mode = ReadRegister(Register.Mode);
                if((mode & (ushort)Mode.Cancel) == 0)
                    break;

                repeat++;
                if(repeat > (2048 / BlockSize))
                {
                    Reset();
                    break;
                }
            }

            if(cancelled)
            {
                for(var i = 0; i <= 2052 / BlockSize; i++)
                    WriteData(buffer);
            }

            Reset();

            playStream.Close();
            OnPlayFinished();
        }

        
        public void Stop()
        {
            stopRequested = true;
            while(IsPlaying)
                Thread.Sleep(1);
        }
    }
}
