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
using Hellevator.Behavior;
using Hellevator.Behavior.Effects;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Interface;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical
{
    public class Program
    {
        //private static readonly IAudioZone zone1 = new PhysicalAudioZone(FEZ_Pin.Digital.Di37, FEZ_Pin.Digital.Di39, FEZ_Pin.Digital.Di41);
        //private static readonly IAudioZone zone2 = new PhysicalAudioZone(FEZ_Pin.Digital.Di47, FEZ_Pin.Digital.Di49, FEZ_Pin.Digital.Di51);
        //private static readonly IAudioZone zone3 = new PhysicalAudioZone(FEZ_Pin.Digital.Di21, FEZ_Pin.Digital.Di23, FEZ_Pin.Digital.Di25);
        
        //private static Playlist playlist3 = new Playlist(true, "ding1", "ding2");
        //private static Playlist playlist2 = new Playlist(true, "sample");
        //private static Playlist playlist1 = new Playlist(true, "backinblack");

        private static PhysicalHellevator hellevator;

        public static void Main()
        {
            //hellevator = new PhysicalHellevator();

            var rope = new LedRope(SPI.SPI_module.SPI1, 70);

            for(int i = 0; i < 70; i++)
            {
                rope.SetColor(i, new Color(0, 10, 10));
                
            }

            while(true)
            {
                rope.Update();

                Thread.Sleep(10);
            }
            //Debug.EnableGCMessages(false);
            Thread.Sleep(10000);
            //var thread = new Thread(Run);
            //thread.Start();
        }

        private static void Run()
        {
            Script.Run(hellevator);
        }
    }
}
