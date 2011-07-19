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

using System.Threading;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Interface;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical
{
    public class Program
    {
        private static readonly IAudioZone zone1 = new PhysicalAudioZone(FEZ_Pin.Digital.Di37, FEZ_Pin.Digital.Di39, FEZ_Pin.Digital.Di41);
        private static readonly IAudioZone zone2 = new PhysicalAudioZone(FEZ_Pin.Digital.Di47, FEZ_Pin.Digital.Di49, FEZ_Pin.Digital.Di51);
        private static readonly IAudioZone zone3 = new PhysicalAudioZone(FEZ_Pin.Digital.Di21, FEZ_Pin.Digital.Di23, FEZ_Pin.Digital.Di25);
        
        private static Playlist playlist3 = new Playlist(true, "ding1", "ding2");
        private static Playlist playlist2 = new Playlist(true, "sample");
        private static Playlist playlist1 = new Playlist(true, "backinblack");

        public static void Main()
        {
            var button = new InputPort((Cpu.Pin) FEZ_Pin.Digital.LDR, false, Port.ResistorMode.PullUp);
            Debug.Print("Starting");

            zone1.Play(playlist1);
            Thread.Sleep(2000);
            zone2.Play(playlist2);
            Thread.Sleep(3000);
            zone3.Play(playlist3);

            //while(true)
            //{
            //    zone3.Play(playlist3);
            //    Thread.Sleep(3000);
            //}

            Thread.Sleep(Timeout.Infinite);
            Debug.Print("Done");
        }

        
    }
}
