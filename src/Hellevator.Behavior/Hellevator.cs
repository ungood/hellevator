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
            Display("RESET");
            
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
            hw.RopeLight.On();
            Pause(1);
            hw.DriveWheel.On();

            Pause(10);

            hw.DriveWheel.Off();
            ceiling.Set(Colors.White);
            hw.CarriageDoor.Open()
                .WaitOne();
            
            // Wait for guest to press the panel button.
            hw.PanelButton
                .Wait();
            
            // Wait for the doors to close before getting underway.
            hw.CarriageDoor.Close()
                .WaitOne();
        }

        #region Heaven

        /// <summary>
        /// Take the guest on the stairway to heaven.
        /// </summary>
        public static void GotoHeaven()
        {
            //hw.EffectsZone.Play(Playlist.ElevatorMusic);
            Goto(Location.Heaven, 30, new ExponentialEase(1.1));
            Goto(Location.Space, 20, new LinearEase());

            // TODO
            //hw.MoodLight.Send(Colors.Blue);
            hw.CarriageDoor.Open()
                .WaitOne();

            hw.PanelButton.Wait();
            hw.CarriageDoor.Close()
                .WaitOne();
        }

        public static void ExitHeaven()
        {
            hw.CarriageDoor.Close();
            Goto(Location.Heaven, 20, new LinearEase());
            Goto(Location.BlackRockCity, 30, new ExponentialEase(1.1));
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
            
            hw.CarriageDoor.Open()
                .WaitOne();

            //hw.MoodLight.Send(Colors.White);

            hw.PanelButton
                .Wait();
            hw.CarriageDoor.Close()
                .WaitOne();
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

            hw.CarriageDoor.Close()
                .WaitOne();
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
            hw.BeginDestination(destination);
            
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
                Set = v => CurrentFloor = v
            };
            animator.Animate();
        }

        private static double currentFloor;
        
        public static double CurrentFloor
        {
            get { return currentFloor; }
            set
            {
                currentFloor = value;
                hw.FloorIndicator.CurrentFloor = (int) Math.Round(value);
            }
        }

        public static void BeginScenario(Scenario scenario)
        {
            hw.BeginScenario(scenario.Name);
            scenario.Run();
        }

        public static void Display(string message)
        {
            hw.Display(message);
        }
    }
}