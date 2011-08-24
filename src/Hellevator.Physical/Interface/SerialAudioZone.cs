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
using Hellevator.Physical.Components;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Interface
{
    public class SerialAudioZone : IAudioZone
    {
        private readonly AudioControllerCoordinator coord;
        private readonly byte address;
        private readonly InputPort isPlaying;

        public SerialAudioZone(AudioControllerCoordinator coord, byte address, FEZ_Pin.Digital isPlayingPin)
        {
            this.coord = coord;
            this.address = address;
            isPlaying = new InputPort((Cpu.Pin)isPlayingPin, false, Port.ResistorMode.Disabled);
        }


        public void Play(string file)
        {
            coord.SendCommand(address, AudioControllerCommand.Play, file);
        }

        public void Loop(string file)
        {
            coord.SendCommand(address, AudioControllerCommand.Loop, file);
        }

        public void Stop(bool fade)
        {
            var command = fade ? AudioControllerCommand.Fade : AudioControllerCommand.Stop;
            coord.SendCommand(address, command);
        }

        public void Wait()
        {
            while(isPlaying.Read())
                Thread.Sleep(10);
        }
    }
}
