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
    internal static class Hellevator
    {
        private static IHellevator hw;
        private static EffectPlayer elevator;
        private static SolidColorPlayer ceiling;
        
        public static void Initialize(IHellevator hardware)
        {
            hw = hardware;
            elevator = new EffectPlayer(hardware.ElevatorEffects);
            ceiling = new SolidColorPlayer(hardware.CeilingEffects);

            hw.PanelButton.Pressed += PanelButtonPressed;
        }

        private static void PanelButtonPressed()
        {
            hw.EffectsZone.Play(Playlist.Beep);
        }

        private static void Pause(double seconds)
        {
            Thread.Sleep((int) (1000 * seconds));
        }

        /// <summary>
        /// Begin attract mode.
        /// </summary>
        public static void Reset(bool firstTime)
        {
            hw.DisplayScenario("RESET");
            hw.DisplayDestination("");
            hw.DisplayInstruction("");

            hw.PatriotLight.Off();
            hw.Fan.Off();
            hw.DriveWheel.Off();
            hw.RopeLight.Off();

            elevator.Off();
            ceiling.Off();
            CurrentFloor = 1;

            hw.LobbyZone.Stop();
            hw.CarriageZone.Stop();
            hw.EffectsZone.Stop();
        }

        /// <summary>
        /// Wait for the guest to get in the damned carriage.
        /// </summary>
        public static void AcceptGuest()
        {
            Pause(5);
            hw.DriveWheel.On();
            Pause(3);
            hw.RopeLight.On();

            Pause(22);
            hw.DriveWheel.Off();

            CurrentFloor = 1;
            hw.PatriotLight.White();
            ceiling.Set(Colors.White);

            hw.CarriageDoor.Open();

            hw.RopeLight.Off();
            hw.PanelButton
                .Wait();
            // CeilingLights = Gold color
            hw.CarriageDoor.Close();
        }

        #region Heaven

        /// <summary>
        /// Take the guest on the stairway to heaven.
        /// </summary>
        public static void GotoHeaven()
        {
            hw.PatriotLight.Off();
            Goto(Location.Heaven, 30, new ExponentialEase(1.1) {Mode = EasingMode.In});
            Goto(Location.Space, 20, new LinearEase());

            hw.PatriotLight.Blue();
            hw.CarriageDoor.Open();

            hw.PanelButton.Wait();
            hw.PatriotLight.Red();
            hw.CarriageDoor.Close();
            hw.PatriotLight.Off();
        }

        public static void ExitHeaven()
        {
            hw.CarriageDoor.Close();
            hw.PatriotLight.Off();
            
            Goto(Location.Heaven, 20, new ExponentialEase(1.1) {Mode = EasingMode.In});
            Goto(Location.BlackRockCity, 30, new ExponentialEase(1.1) { Mode = EasingMode.Out});
            hw.CarriageDoor.Open();
        }

        #endregion 

        #region Purgatory

        /// <summary>
        /// Send the guest to land of the half-damned.
        /// </summary>
        public static void GotoPurgatory()
        {
            Goto(Location.MidPurgatory, 30, new BounceEase {Mode = EasingMode.In});
            Goto(Location.Purgatory, 30, new BounceEase {Mode = EasingMode.Out});

            hw.CarriageDoor.Open();

            //hw.MoodLight.Send(Colors.White);

            hw.PanelButton
                .Wait();
            hw.CarriageDoor.Close();
        }

        public static void ExitPurgatory()
        {
            elevator.Play(new WhiteNoiseEffect());
            Thread.Sleep(20 * 1000);
        }

        #endregion

        #region Hell

        /// <summary>
        /// TONIGHT WE DINE IN HELL!
        /// </summary>
        public static void GotoHell()
        {
            Goto(Location.Hell1, 20, new ExponentialEase {Mode = EasingMode.In});
            Thread.Sleep(2 * 1000);
            Goto(Location.Hell2, 20, new LinearEase());
            //hw.Fan.On();
            ////hw.FloorIndicator.StartFlicker();
            ////elevatorEffectsPlayer.Play();

            //hw.Fan.Off();
            //hw.HellLights.On();
            //hw.CarriageDoor.Open();
            //hw.Chandelier.Off();

            hw.CarriageDoor.Close();
        }

        /// <summary>
        /// Do not pass GO, do not collect $200.
        /// </summary>
        public static void ExitHell()
        {
            CurrentFloor = Location.Hell1.GetFloor();
            Goto(Location.BlackRockCity, 30, new ExponentialEase(1.1));
        }

        #endregion
        
        private static void WaitAll(params WaitHandle[] handles)
        {
            WaitHandle.WaitAll(handles);
        }
        
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
        private static double delta = 0;

        private static void SetCurrentFloor(double value, long ticks, bool final)
        {
            var seconds = (double)ticks / TimeSpan.TicksPerSecond;
            delta = value - currentFloor;
            var floorsPerSecond = delta / seconds;
            CurrentFloor = value;

            if(final)
                hw.Fan.Off();
            else if(floorsPerSecond < -2) // High speed cutoff
                hw.Fan.High();
            else if(floorsPerSecond < -1) // Low speed cutoff
                hw.Fan.Low();
            else
                hw.Fan.Off();
        }

        public static double CurrentFloor
        {
            get { return currentFloor; }
            set
            {
                currentFloor = value;
                hw.FloorIndicator.CurrentFloor = (int) Math.Round(value);
            }
        }

        public static void DisplayScenario(Scenario scenario)
        {
            hw.DisplayScenario(scenario.Name);
        }
    }
}