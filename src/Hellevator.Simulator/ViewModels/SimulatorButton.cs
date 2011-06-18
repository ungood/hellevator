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
using System.Windows.Input;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    

    public class SimulatorButton : IButton
    {
        public ICommand Click { get; private set; }

        public SimulatorButton()
        {
            Click = new RelayCommand(Execute);
        }

        private void Execute(object obj)
        {
            pressed.Set();
        }

        private readonly AutoResetEvent pressed = new AutoResetEvent(false);
        public WaitHandle Pressed
        {
            get { return pressed; }
        }
    }
}