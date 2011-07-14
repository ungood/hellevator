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

namespace Hellevator.Physical.Components
{
    public class AudioShieldPlayer : AudioShield
    {
        public bool IsPlaying { get; private set; }

        private bool stopRequested;
        private Stream playStream;
        private Thread thread;
        private readonly byte[] block = new byte[BlockSize];

        public AudioShieldPlayer(SpiCoordinator coordinator, Cpu.Pin dataSelectPin, Cpu.Pin cmdSelectPin,
            Cpu.Pin dreqPin)
            : base(coordinator, dataSelectPin, cmdSelectPin, dreqPin) {}

        public void Play(Stream stream)
        {
            stopRequested = false;
            IsPlaying = true;
            playStream = stream;
            thread = new Thread(PlayStreamTask);
            thread.Start();
        }

        private void PlayStreamTask()
        {
            while(true)
            {
                if(stopRequested)
                    break;

                var bytesRead = playStream.Read(block, 0, BlockSize);
                if(bytesRead < 1)
                    break;

                WaitForDreq();

                Coordinator.Execute(DataConfig, spi => spi.Write(block));
            }

            Reset();
        }

        public void Stop()
        {
            stopRequested = true;
        }
    }
}