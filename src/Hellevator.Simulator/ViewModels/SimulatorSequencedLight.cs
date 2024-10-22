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

using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Effects;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorSequencedLight : ViewModelBase
    {
        private Color color = Colors.Black;

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }

        public void TurnOff()
        {
            Color = Colors.Black;
        }

        public void Send(ushort data)
        {
            Color = (Color) data;
        }
    }
}
