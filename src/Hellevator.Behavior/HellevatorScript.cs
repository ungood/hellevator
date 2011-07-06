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
    public class HellevatorScript
    {
        private readonly ScenarioLoop loop = new ScenarioLoop {
            RandomScenario.Instance,
            HeavenHellScenario.Instance,
            HeavenScenario.Instance,
            PurgatoryScenario.Instance,
            HellScenario.Instance
        };

        public bool IsScenarioRunning { get; private set; }

        public HellevatorScript(IHellevator hellevator)
        {
            Hellevator.Initialize(hellevator);
            Hellevator.ModeButton.Pressed += ModeButtonPressed;
        }

        public void Run()
        {
            Reset(true);
            while(true)
            {    
                Hellevator.CallButton.Wait();
                loop.Current.Run();
                Reset(false);
            }
        }

        private void ModeButtonPressed()
        {
            if(IsScenarioRunning)
            {
                EmergencyReset();
            }
            else
            {
                loop.Next();
                Hellevator.Debug.Print(1, loop.Current.Name);
            }
        }

        private void EmergencyReset()
        {
            
        }

        private void Reset(bool firstTime)
        {
            if(!firstTime)
                Thread.Sleep(10 * 1000);
            else
                Hellevator.Debug.Print(1, loop.Current.Name);

            Hellevator.CarriageDoor.Close();
            Hellevator.Chandelier.TurnOff();
            Hellevator.CarriageZone.Stop();
            Hellevator.InsideZone.Stop();
            Hellevator.Fan.TurnOff();
            Hellevator.HellLights.TurnOff();
            Hellevator.VerticalChasePlayer.Stop();
            Hellevator.PanelPlayer.Stop();
            
            Hellevator.CurrentFloor = Location.Entrance.GetFloor();
            
            Hellevator.Turntable.Reset();
        }
    }
}