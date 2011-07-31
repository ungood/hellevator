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

using System.Collections.ObjectModel;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorFloorIndicator : ViewModelBase, IFloorIndicator
    {
        public ObservableCollection<Light> Lights { get; private set; }
        public int NumLights { get; private set; }
        private int currentFloor;

        public SimulatorFloorIndicator(int numLights)
        {
            NumLights = numLights;
            Lights = new ObservableCollection<Light>();
            for(var i = 0; i < NumLights; i++)
                Lights.Add(new Light());
            CurrentFloor = 7;
        }

        public int CurrentFloor
        {
            set
            {
                if(value == currentFloor)
                    return;

                currentFloor = value;
                Stopwatch.Print("Floor {0}", value);

                foreach(var light in Lights)
                    light.IsOn = false;

                if(value > NumLights)
                {
                    Lights[0].IsOn = value % 2 == 0;
                    return;
                }

                if(value < 1)
                    return;

                Lights[24 - value].IsOn = true;
            }
        }

        public class Light : ViewModelBase
        {
            private bool isOn;

            public bool IsOn
            {
                get { return isOn; }
                set
                {
                    if(value == isOn)
                        return;

                    isOn = value;
                    OnPropertyChanged("IsOn");
                }
            }
        }
    }
}
