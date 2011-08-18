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
using Microsoft.SPOT;

namespace Hellevator.Physical
{
    public static class Program
    {
        public static readonly PhysicalHellevator TheBox = new PhysicalHellevator();

        public static void Main()
        {
            Debug.EnableGCMessages(false);

            var thread = new Thread(Run);
            thread.Start();

            CycleSmoke();
        }

        private static void Run()
        {
            Script.Run(TheBox);
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

        private static void LightTest()
        {
            while(true)
            {
                TheBox.PatriotLight.Red();
                Thread.Sleep(1000);
                TheBox.PatriotLight.White();
                Thread.Sleep(1000);
                TheBox.PatriotLight.Blue();
                Thread.Sleep(1000);
                TheBox.PatriotLight.Off();
                Thread.Sleep(3000);

                TheBox.Fan.Low();
                Thread.Sleep(1000);
                TheBox.Fan.High();
                Thread.Sleep(1000);
                TheBox.Fan.Off();
                Thread.Sleep(3000);

                TheBox.RopeLight.On();
                Thread.Sleep(1000);
                TheBox.RopeLight.Off();
                Thread.Sleep(1000);

                TheBox.DriveWheel.On();
                Thread.Sleep(1000);
                TheBox.DriveWheel.Off();
                Thread.Sleep(1000);

                TheBox.SmokeMachine.On();
                Thread.Sleep(1000);
                TheBox.SmokeMachine.Off();
                Thread.Sleep(1000);
            }
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
