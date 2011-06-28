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

using System.Collections;
using Hellevator.Behavior.Animations;

namespace Hellevator.Behavior.Scenarios
{
    public abstract class Scenario
    {
        public abstract string Name { get; }
        public abstract void Run();

        /// <summary>
        /// Guest has pressed the call button, wait for them to get in the damned carriage.
        /// </summary>
        protected void WaitForGuest()
        {
            Hellevator.Debug.Print(2, "WAIT GUEST");

            Hellevator.CurrentFloor = Location.Entrance.GetFloor();
            Hellevator.CarriageDoor.Open();
            Hellevator.Chandelier.TurnOn();
            Hellevator.PanelButton.Pressed.WaitOne();
        }

        /// <summary>
        /// Take the guest on the stairway to heaven.
        /// </summary>
        protected void GoToHeaven()
        {
            Hellevator.Debug.Print(2, "TO: HEAVEN");

            Hellevator.CarriageDoor.Close();
            Hellevator.CarriageZone.Play("TheGirl");
            Hellevator.Goto(Location.Heaven, 1500);
            Hellevator.CarriageZone.Stop();
            Hellevator.CarriageDoor.Open();
        }

        /// <summary>
        /// Send the guest to land of the half-damned.
        /// </summary>
        protected void GoToPurgatory()
        {
            Hellevator.Debug.Print(2, "TO: PURGATORY");

            Hellevator.CarriageDoor.Close();
            Hellevator.CarriageZone.Play("Kalimba");
            Hellevator.InsideZone.Play("Sleep Away");
            Hellevator.Goto(Location.Purgatory, 1000);
            Hellevator.CarriageDoor.Open();
        }

        /// <summary>
        /// TONIGHT WE DINE IN HELL!
        /// </summary>
        protected void GoToHell()
        {
            Hellevator.Debug.Print(2, "TO: HELL");

            Hellevator.CarriageZone.Stop();
            Hellevator.InsideZone.Stop();

            Hellevator.CarriageDoor.Close();
            Hellevator.Fan.TurnOn();
            Hellevator.Goto(Location.Hell, 300, new ExponentialEase(5) { Mode = EasingMode.In });
            Hellevator.Fan.TurnOff();
            Hellevator.HellLights.TurnOn();
            Hellevator.CarriageDoor.Open();
            Hellevator.Chandelier.TurnOff();
        }

        /// <summary>
        /// Do not pass GO, do not collect $200.
        /// </summary>
        protected void GoToExit()
        {
            Hellevator.Debug.Print(2, "TO: EXIT");

            Hellevator.CarriageDoor.Close();
            Hellevator.Goto(Location.BlackRockCity, 1500);
            Hellevator.CarriageDoor.Open();
        }
    }
}