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
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.IO;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Components;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Interface
{
    public class PhysicalAudioZone : IAudioZone
    {
        private static readonly PersistentStorage Storage
            = new PersistentStorage("SD");

        private static readonly SpiCoordinator Coordinator
            = new SpiCoordinator(SPI.SPI_module.SPI2);

        private readonly AudioShieldPlayer player;
        
        static PhysicalAudioZone()
        {
            Storage.MountFileSystem();
        }

        public PhysicalAudioZone(FEZ_Pin.Digital dataPin, FEZ_Pin.Digital cmdPin, FEZ_Pin.Digital dreqPin)
        {
            player = new AudioShieldPlayer(
                Coordinator,
                (Cpu.Pin) dataPin,
                (Cpu.Pin) cmdPin,
                (Cpu.Pin) dreqPin);

            player.SetVolume(255, 255);
        }

        public WaitHandle Play(Playlist playlist)
        {
            var stream = new FileStream(@"\SD\" + playlist.GetNext() + ".ogg", FileMode.Open);
            player.Play(stream);

            return new ManualResetEvent(true);
        }

        public void Loop(Playlist playlist)
        {
            //throw new System.NotImplementedException();
        }

        WaitHandle IAudioZone.Stop()
        {
            return new ManualResetEvent(true);
            // TODO
        }

    }
}
