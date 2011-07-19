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

using System;
using System.Threading;
using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Interface;
using Hellevator.Behavior.Scenarios;

namespace Hellevator.Behavior
{
    public static class Script
    {
        private static readonly ScenarioLoop loop = new ScenarioLoop {
            RandomScenario.Instance,
            HeavenHellScenario.Instance,
            HeavenScenario.Instance,
            PurgatoryScenario.Instance,
            HellScenario.Instance
        };
        public static bool IsScenarioRunning;
        
        public static void Run(IHellevator hardware)
        {
            Hellevator.Initialize(hardware);
            hardware.ModeButton.Pressed += ModeButtonPressed;

            Hellevator.Reset(true);
            while(true)
            {
                hardware.CallButton.Wait();
                loop.Current.Run();
                Hellevator.Reset(false);
            }
        }

        private static void ModeButtonPressed()
        {
            if(IsScenarioRunning)
            {
                Hellevator.EmergencyStop();
            }
            else
            {
                loop.Next();
                Hellevator.Display(1, loop.Current.Name);
            }
        }
    }
}