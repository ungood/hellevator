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
using Hellevator.Behavior.Interface;
using Hellevator.Behavior.Scenarios;

namespace Hellevator.Behavior
{
    public static class Script
    {
        public static bool IsScenarioRunning;

        private static readonly ScenarioLoop Loop = new ScenarioLoop {
            SequenceScenario.Instance,
            HeavenScenario.Instance,
            PurgatoryScenario.Instance,
            HellScenario.Instance,
            DemoScenario.Instance
        };

        
        public static void Run(IHellevator hardware)
        {
            Hellevator.Initialize(hardware);
            hardware.ModeButton.Pressed += ModeButtonPressed;

            Hellevator.Reset(true);
            Hellevator.DisplayScenario(Loop.Current);
            while(true)
            {
                Hellevator.Reset(false);
                RunScenario();
            }
        }
        
        private static void RunScenario()
        {
            Hellevator.AcceptGuest();

            IsScenarioRunning = true;
            Hellevator.DisplayScenario(Loop.Current);
            Loop.Current.Run();
            
            Thread.Sleep(30 * 1000);
            Hellevator.Reset(false);
            IsScenarioRunning = false;
        }

        private static void ModeButtonPressed()
        {
            Loop.Next();
            Hellevator.DisplayScenario(Loop.Current);
            Thread.Sleep(250);
        }
    }
}