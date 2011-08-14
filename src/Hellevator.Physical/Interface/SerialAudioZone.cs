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

using System.IO.Ports;
using System.Threading;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;

namespace Hellevator.Physical.Interface
{
    public class SerialAudioZone : IAudioZone
    {
        //private static readonly PersistentStorage Storage
        //    = new PersistentStorage("SD");

        //private static readonly SpiCoordinator Coordinator
        //    = new SpiCoordinator(SPI.SPI_module.SPI1);

        //private readonly ShieldPlayer player;

        public SerialAudioZone(SerialPort serial, byte address)
        {
            //player = new AudioShieldPlayer(
            //    Coordinator,
            //    (Cpu.Pin) dataPin,
            //    (Cpu.Pin) cmdPin,
            //    (Cpu.Pin) dreqPin);

            //player.SetVolume(255, 255);
        }

        public WaitHandle Play(Playlist playlist)
        {
            //var stream = new FileStream(@"\SD\" + playlist.GetNext() + ".ogg", FileMode.Open);
            //player.Play(stream);

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
