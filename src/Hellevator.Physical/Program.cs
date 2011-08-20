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
using Hellevator.Behavior;
using Hellevator.Behavior.Effects;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Interface;
using Microsoft.SPOT;

namespace Hellevator.Physical
{
    public static class Program
    {
        public static readonly PhysicalHellevator TheBox = new PhysicalHellevator();

        public static void Main()
        {
            Debug.EnableGCMessages(false);
            
            TheBox.DisplayScenario("SCENARIO");
            TheBox.DisplayDestination("DESTINATION");
            TheBox.DisplayInstruction("INSTRUCTION");

            var thread = new Thread(Run);
            thread.Start();
            
            CycleSmoke();
        }

        private static void Run()
        {
            Behavior.Hellevator.Initialize(TheBox);
            Behavior.Hellevator.Reset(true);
            Behavior.Hellevator.CurrentFloor = Location.Heaven.GetFloor();
            Behavior.Hellevator.ExitHeaven();
            //Script.Run(TheBox);
        }

        private static void FloorIndicatorTest()
        {
            var i = 1;
            while(true)
            {
                TheBox.FloorIndicator.CurrentFloor = i;
                i++;
                if(i > 24)
                    i = 1;
                Thread.Sleep(1000);
            }
        }

        private static void RelayTest()
        {
            while(true)
            {
                TheBox.PatriotLight.Red();
                Thread.Sleep(1000);
                TheBox.PatriotLight.White();
                Thread.Sleep(1000);
                TheBox.PatriotLight.Blue();
                Thread.Sleep(3000);

                Cycle(TheBox.Fan);
                Cycle(TheBox.RopeLight);
                Cycle(TheBox.DriveWheel);
                Cycle(TheBox.SmokeMachine);
            }
        }

        private static void Cycle(IRelay relay)
        {
            relay.On();
            Thread.Sleep(1000);
            relay.Off();
            Thread.Sleep(1000);
        }

        private static void Pause(int minute, int second)
        {
            for(var i = 0; i < (minute * 60) + second; i++)
                Thread.Sleep(1000);
        }

        private static void CycleSmoke()
        {
            // Heat it up
            TheBox.SmokeMachine.On();
            Pause(4, 30);

            while(true)
            {
                TheBox.SmokeMachine.Off();
                Pause(2, 0);

                TheBox.SmokeMachine.On();
                Pause(0, 30);
            }
        }
    }
}
