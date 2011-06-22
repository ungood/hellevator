﻿#region License
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorFloorIndicator : ViewModelBase, IFloorIndicator
    {
        
        private double floor;

        public double Floor
        {
            get { return floor; }
            set
            {
                floor = value;
                this.UpdateLights();
            }
        }

        public ObservableCollection<LightViewModel> Lights { get; private set; }

        public SimulatorFloorIndicator()
        {
            Lights = new ObservableCollection<LightViewModel>();
            for(int i = 0; i < Floors.Total; i++)
                Lights.Add(new LightViewModel());
            
            Floor = 1;
        }

        public void Flicker()
        {
            // TODO
        }

        public void TurnOff()
        {
            foreach(var light in Lights)
                light.Intensity = 0;
        }

        public void SetLight(int i, double intensity)
        {
            Lights[i-1].Intensity = intensity;
        }
    }

    public class LightViewModel : ViewModelBase
    {
        private double intensity;

        public double Intensity
        {
            get { return intensity; }
            set
            {
                if(value == intensity)
                    return;

                intensity = value;
                OnPropertyChanged("Intensity");
            }
        }
    }
}
