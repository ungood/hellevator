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
using Hellevator.Behavior.Effects;

namespace Hellevator.Physical
{
    public class Program
    {
        // NOTE: DONT ERASE THIS
        
        
        
        //private static Playlist playlist3 = new Playlist(true, "ding1", "ding2");
        //private static Playlist playlist2 = new Playlist(true, "sample");
        //private static Playlist playlist1 = new Playlist(true, "backinblack");

        private static readonly PhysicalHellevator hellevator = new PhysicalHellevator();

        //private static LedRope rope = new LedRope(SPI.SPI_module.SPI1, 70);



        public static void Main()
        {
            var thread = new Thread(Run);
            thread.Start();

            CycleSmoke();
        }

        private static void Run()
        {
            var player = new EffectPlayer(hellevator.ElevatorEffects);
            while(true)
            {
                player.Play(new RainbowEffect(1, 5));
                hellevator.PatriotLight.Blue();

                Thread.Sleep(30 * 1000);

                player.Play(new HellEffect());
                hellevator.PatriotLight.Red();

                Thread.Sleep(30 * 1000);
            }

            //var i = 0;
            //while(true)
            //{
            //    hellevator.CallButton.Wait();
            //    i++;
            //    if(i > 24)
            //        i = 1;
            //    hellevator.FloorIndicator.CurrentFloor = i;

            //}
        }

        private static void LightTest()
        {
            while(true)
            {
                hellevator.PatriotLight.Red();
                Thread.Sleep(1000);
                hellevator.PatriotLight.White();
                Thread.Sleep(1000);
                hellevator.PatriotLight.Blue();
                Thread.Sleep(1000);
                hellevator.PatriotLight.Off();
                Thread.Sleep(1000);
            }
        }

        private static void Pause(int minute, int second)
        {
            for(int i = 0; i < (minute * 60) + second; i++)
                Thread.Sleep(1000);
        }

        private static void CycleSmoke()
        {
            // Heat it up
            hellevator.SmokeMachine.On();
            Pause(4, 30);

            while(true)
            {
                hellevator.SmokeMachine.Off();
                Pause(2, 0);

                hellevator.SmokeMachine.On();
                Pause(0, 30);
            }
        }
    }
}
