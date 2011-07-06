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
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.FEZ.Shields;
using GHIElectronics.NETMF.IO;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Interface
{
    public class PhysicalAudioZone : IAudioZone
    {
        private readonly MusicShield audio = new MusicShield(
                SPI.SPI_module.SPI1,
                FEZ_Pin.Digital.An4,
                FEZ_Pin.Digital.An5,
                FEZ_Pin.Digital.Di4);
        private readonly PersistentStorage sd = new PersistentStorage("SD");

        public PhysicalAudioZone()
        {
            audio.SetVolume(255, 255);
            sd.MountFileSystem();
        }

        public void Play(string filename)
        {
            using(var stream = new FileStream(@"\SD\sample.ogg", FileMode.Open))
            {
                audio.Play(stream);
            }
        }

        public void Stop()
        {
            audio.StopPlaying();
        }
    }
}
