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

namespace Hellevator.Behavior.States
{
    /// <summary>
    /// This state begins when the guest presses the call button, and ends
    /// when the guest enters the carriage and presses the panel button.
    /// </summary>
    /// <remarks>
    /// Assumptions:
    /// * Turntable is at "Heaven"
    /// * Outer doors have been opened.
    /// </remarks>
    public class CallButtonPressed : State
    {
        protected override void Enter()
        {
            InsideZone.Play("CallButtonPressed_Inside");
            CarriageZone.Play("CallButtonPressed_Carriage");
            Chandelier.TurnOn();
            FloorIndicator.Floor = Floors.BlackRockCity;
            CarriageDoor.Open();
        }

        protected override WaitHandle[] WaitHandles
        {
            get { return new[] {PanelButton.Pressed}; }
        }
    }
}
