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
using Hellevator.Behavior.Effects;
using Hellevator.Behavior.Interface;
using Hellevator.Behavior.Scenarios;
using Microsoft.SPOT;
using Math = System.Math;

namespace Hellevator.Behavior
{
    public static class Hellevator
    {
        private static IHellevator hw;
        private static EffectPlayer elevator;
        private static SolidColorPlayer ceiling;
        
        public static void Initialize(IHellevator hardware)
        {
            hw = hardware;
            elevator = new EffectPlayer(hardware.ElevatorEffects);
            ceiling = new SolidColorPlayer(hardware.CeilingEffects);
        }

        private static void Pause(double seconds)
        {
            Thread.Sleep((int) (1000 * seconds));
        }

        private static DateTime mark = DateTime.Now;

        private static void Mark(string note)
        {
            mark = DateTime.Now;
            var interval = TimeSpan.Zero;
            Debug.Print("| " + interval.ToString() + " | " + note + " | |");
        }

        private static void Time(string note)
        {
            var interval = DateTime.Now - mark;
            Debug.Print("| " + interval.ToString() + " | " + note + " | |");
        }

        private const int AudioOffset = 200;

        /// <summary>
        /// Begin attract mode.
        /// </summary>
        public static void Reset(bool firstTime)
        {
            hw.DisplayDestination("BRC");
            hw.DisplayInstruction("");

            hw.Fan.Off();
            hw.DriveWheel.Off();
            hw.RopeLight.Off();

            elevator.Off();
            ceiling.Off();
            CurrentFloor = 1;

            hw.ExteriorZone.Stop(false);
            hw.InteriorZone.Stop(false);
            hw.PatriotLight.Off();
        }

        /// <summary>
        /// Wait for the guest to get in the damned carriage.
        /// </summary>
        public static void AcceptGuest()
        {
            hw.CallButton.Wait();
            hw.ExteriorZone.Play(Playlists.Accept.TravelExterior);
            hw.DisplayInstruction("CALL BUTTON PRESSED");
            Mark("Call Button Pressed");

            Pause(5);
            hw.DriveWheel.On();
            Time("Drive Wheel On");

            Pause(3);
            hw.RopeLight.On();
            Time("Rope Light On");

            Pause(22);
            hw.DriveWheel.Off();
            Time("Drive Wheel Off");

            CurrentFloor = 1;
            ceiling.Set(Colors.White);
            hw.PatriotLight.White();

            Time("White Light On, Open Doors");

            hw.CarriageDoor.Open();
            hw.ExteriorZone.Loop(Playlists.Idle.DestinationExterior);
            hw.InteriorZone.Loop(Playlists.Accept.DestintationInterior);
            
            hw.RopeLight.Off();
            Time("Rope Light Off, Waiting on Panel Button");

            hw.PanelButton.Wait();
            hw.InteriorZone.Stop(true);
            hw.ExteriorZone.Stop(true);
            hw.InteriorZone.Wait();
        }

        #region Heaven

        /// <summary>
        /// Take the guest on the stairway to heaven.
        /// </summary>
        public static void GotoHeaven()
        {
            Mark("Panel Button Pressed, Closing Doors");
            ceiling.Off();
            hw.InteriorZone.Play(Playlists.Heaven.GotoInterior);
            hw.ExteriorZone.Play(Playlists.Heaven.GotoExterior);
            Thread.Sleep(AudioOffset);
            hw.CarriageDoor.Close();
            hw.PatriotLight.Off();

            Time("Doors Closed, Going Up");
            hw.RopeLight.On();
            Goto(Location.TopFloor, 30, new ExponentialEase(1.1) {Mode = EasingMode.In});
            
            Time("Breaking through top floor");
            Goto(Location.Heaven, 20, new ExponentialEase(1.1) {Mode = EasingMode.Out});
            hw.RopeLight.Off();

            Time("Arrived in Heaven, Open Doors");
            hw.PatriotLight.Blue();
            ceiling.Set(Colors.Blue);
            hw.CarriageDoor.Open(false);
            hw.InteriorZone.Wait();

            Time("Doors Open, Waiting on Panel Button");
            hw.InteriorZone.Loop(Playlists.Heaven.DestinationInterior);
            hw.ExteriorZone.Loop(Playlists.Idle.DestinationExterior);
            hw.PanelButton.Wait();
            hw.InteriorZone.Stop(true);
            hw.ExteriorZone.Stop(true);
            hw.ExteriorZone.Wait();
        }

        public static void ExitHeaven()
        {
            Mark("Panel Button Pressed, Closing Doors");
            hw.InteriorZone.Play(Playlists.Heaven.ExitInterior);
            hw.ExteriorZone.Play(Playlists.Heaven.ExitExterior);
            Thread.Sleep(AudioOffset);
            hw.CarriageDoor.Close();
            hw.PatriotLight.Off();
            ceiling.Off();
            
            Time("Doors Closed, Going Down");
            hw.RopeLight.On();
            Goto(Location.TopFloor, 20, new ExponentialEase(1.1) {Mode = EasingMode.In});
            
            Time("Breaking through top floor");
            Goto(Location.BlackRockCity, 30, new ExponentialEase(1.1) { Mode = EasingMode.Out});
            hw.RopeLight.Off();
            
            Time("Arrived at BRC, Open Doors");
            hw.PatriotLight.White();
            hw.CarriageDoor.Open();
            hw.InteriorZone.Stop(true);
            hw.ExteriorZone.Loop(Playlists.Idle.DestinationExterior);
            
            Time("Doors Open, Sequence Done");
        }

        #endregion 

        #region Purgatory

        /// <summary>
        /// Send the guest to land of the half-damned.
        /// </summary>
        public static void GotoPurgatory()
        {
            Mark("Panel Button Pressed, Closing Doors");
            ceiling.Off();
            hw.InteriorZone.Play(Playlists.Purgatory.GotoInterior);
            hw.ExteriorZone.Play(Playlists.Purgatory.GotoExterior);
            Thread.Sleep(AudioOffset);
            hw.CarriageDoor.Close();
            hw.PatriotLight.Off();

            Time("Doors Closed, Going Up?");
            hw.RopeLight.On();
            Goto(Location.MidPurgatory, 30, new BounceEase {Mode = EasingMode.In});

            Time("Mid-point");
            Goto(Location.Purgatory, 30, new BounceEase {Mode = EasingMode.Out});
            hw.RopeLight.Off();

            Time("Arrived at Purgatory, Play Static, Open Doors");
            hw.PatriotLight.White();
            ceiling.Set(Colors.White);
            elevator.Play(new WhiteNoiseEffect());

            hw.CarriageDoor.Open(false);
            hw.InteriorZone.Wait();
            hw.InteriorZone.Loop(Playlists.Purgatory.DestinationInterior);
            hw.ExteriorZone.Loop(Playlists.Idle.DestinationExterior);

            Time("Doors Open, Waiting on Panel Button");
            hw.PanelButton.Wait();
            hw.InteriorZone.Stop(true);
            hw.ExteriorZone.Stop(true);
            hw.ExteriorZone.Wait();
        }

        public static void ExitPurgatory()
        {
            Mark("Panel Button Pressed, Closing Doors");
            hw.InteriorZone.Play(Playlists.Purgatory.ExitInterior);
            hw.ExteriorZone.Play(Playlists.Purgatory.ExitExterior);
            Thread.Sleep(AudioOffset);
            hw.CarriageDoor.Close();
            hw.PatriotLight.Off();
            ceiling.Off();

            Time("Doors Closed, Fade Static to Gold");
            // If there is time, fade to GOLD.
            elevator.Play(new WhiteNoiseEffect());
            Pause(20);


            Time("Arrived at BRC, Open Doors");
            hw.PatriotLight.White();
            hw.CarriageDoor.Open();

            hw.InteriorZone.Stop(true);
            hw.ExteriorZone.Loop(Playlists.Idle.DestinationExterior);
            Time("Doors Open, Sequence Done");
        }

        #endregion

        #region Hell

        /// <summary>
        /// TONIGHT WE DINE IN HELL!
        /// </summary>
        public static void GotoHell()
        {
            Mark("Panel Button Pressed, Closing Doors");
            ceiling.Off();
            hw.InteriorZone.Play(Playlists.Hell.GotoInterior);
            hw.ExteriorZone.Play(Playlists.Hell.GotoExterior);
            Thread.Sleep(AudioOffset);
            hw.CarriageDoor.Close();
            hw.PatriotLight.Off();

            Time("Doors Closed, Going Down");
            hw.RopeLight.On();
            Goto(Location.BrokenFloor, 15, new ExponentialEase(1.1));
            
            Time("Elevator 'Breaks', 5 second pause");
            Pause(5);

            Time("Falling to hell");
            Goto(Location.Hell, 20, new LinearEase());
            hw.RopeLight.Off();

            Time("Arrived in Hell, Open Doors");
            hw.PatriotLight.Red();
            ceiling.Set(Colors.Red);
            hw.CarriageDoor.Open(false);

            Time("Doors Open, Wait for Button");
            hw.InteriorZone.Wait();
            hw.InteriorZone.Play(Playlists.Hell.DestinationInterior);
            hw.ExteriorZone.Play(Playlists.Idle.DestinationExterior);
            hw.PanelButton.Wait();
            hw.InteriorZone.Stop(true);
            hw.ExteriorZone.Stop(true);
            hw.ExteriorZone.Wait();
        }

        /// <summary>
        /// Do not pass GO, do not collect $200.
        /// </summary>
        public static void ExitHell()
        {
            Mark("Panel button pressed, closing doors");
            hw.InteriorZone.Play(Playlists.Hell.ExitInterior);
            hw.ExteriorZone.Play(Playlists.Hell.ExitExterior);
            hw.CarriageDoor.Close();
            hw.PatriotLight.Off();
            ceiling.Off();

            Time("Doors closed, Going up");
            CurrentFloor = Location.BrokenFloor.GetFloor();
            hw.RopeLight.On();
            Goto(Location.BlackRockCity, 30, new ExponentialEase(1.1));
            hw.RopeLight.Off();

            Time("Arrived at BRC, Open Doors");
            hw.PatriotLight.White();
            hw.CarriageDoor.Open();
            hw.InteriorZone.Stop(true);
            hw.ExteriorZone.Loop(Playlists.Idle.DestinationExterior);
        }

        #endregion

        #region GOTO

        private static void Goto(Location destination, int duration, EasingFunction easing = null)
        {
            hw.DisplayDestination(destination.GetName());
            
            elevator.Play(new ElevatorEffect());

            var destFloor = destination.GetFloor();

            if(easing == null)
                easing = new ExponentialEase();
            var length = new TimeSpan(0, 0, 0, duration);

            var animator = new Animator {
                InitialValue = CurrentFloor,
                FinalValue = destFloor,
                EasingFunction = easing,
                Length = length,
                Set = SetCurrentFloor
            };
            animator.Animate();
        }

        private static double currentFloor;
        
        private static void SetCurrentFloor(double value, long ticks, bool final)
        {
            CurrentFloor = value;
        }

        public static double CurrentFloor
        {
            get { return currentFloor; }
            set
            {
                currentFloor = value;

                var intFloor = (int) value;
                if(intFloor != hw.FloorIndicator.CurrentFloor)
                {
                    hw.FloorIndicator.CurrentFloor = (int) value;
                    Time("Ding " + intFloor);
                }
            }
        }

        #endregion

        public static void DisplayScenario(Scenario scenario)
        {
            hw.DisplayScenario(scenario.Name);
        }
    }
}