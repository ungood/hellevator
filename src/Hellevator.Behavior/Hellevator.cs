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
        private static EffectPlayer elevatorEffectsPlayer;
        
        public static void Initialize(IHellevator hardware)
        {
            hw = hardware;
            elevatorEffectsPlayer = new EffectPlayer(hardware.ElevatorEffects);

            hw.PanelButton.Pressed += PanelButtonPressed;
        }

        private static void PanelButtonPressed()
        {
            hw.EffectsZone.Play(Playlist.Beep);
        }

        /// <summary>
        /// Begin attract mode.
        /// </summary>
        public static void Reset(bool firstTime)
        {
            Display("RESET");
            // TODO
            hw.CarriageDoor.Close();
            hw.Chandelier.TurnOff();
            hw.CarriageZone.Stop();
            hw.LobbyZone.Stop();
            hw.Fan.TurnOff();
            hw.HellLights.TurnOff();
            
            elevatorEffectsPlayer.Stop();
            CurrentFloor = Location.Entrance.GetFloor();
            
            hw.Turntable.Reset();
        }

        /// <summary>
        /// Wait for the guest to get in the damned carriage.
        /// </summary>
        public static void AcceptGuest()
        {
            hw.Chandelier.TurnOn();
            hw.MoodLight.Send(Colors.White);
            hw.CarriageZone.Loop(Playlist.ElevatorMusic);
            WaitAll(
                hw.CarriageDoor.Open(),
                hw.LobbyZone.Play(Playlist.WarmupSounds));
                
            // Open main doors to accept guest
            hw.LobbyZone.Loop(Playlist.IdleSounds);
            hw.MainDoor.Open()
                .WaitOne();
            
            // Wait for guest to press the panel button.
            hw.PanelButton
                .Wait();
            
            // Wait for the doors to close before getting underway.
            hw.CarriageDoor.Close()
                .WaitOne();
            hw.MainDoor.Close()
                .WaitOne();

            hw.CarriageZone.Stop();
            hw.EffectsZone.Play(Playlist.WelcomeToHellevator)
                .WaitOne();
        }

        /// <summary>
        /// Take the guest on the stairway to heaven.
        /// </summary>
        public static void GotoHeaven()
        {
            Goto(Location.Heaven, 15);
            Goto(Location.Space, 20);

            // TODO
            //hw.MoodLight.Send(Colors.Blue);
            hw.CarriageDoor.Open()
                .WaitOne();

            hw.PanelButton.Wait();
            hw.CarriageDoor.Close()
                .WaitOne();
        }

        /// <summary>
        /// Send the guest to land of the half-damned.
        /// </summary>
        public static void GotoPurgatory()
        {
            Goto(Location.MidPurgatory, 20, EasingFunction.ToMidPurgatory);
            Goto(Location.Purgatory, 20, EasingFunction.ToPurgatory);
            
            hw.CarriageDoor.Open()
                .WaitOne();

            hw.MoodLight.Send(Colors.White);

            hw.PanelButton
                .Wait();
            hw.CarriageDoor.Close()
                .WaitOne();
        }

        /// <summary>
        /// TONIGHT WE DINE IN HELL!
        /// </summary>
        public static void GotoHell()
        {
            Goto(Location.Hell, 30, new ExponentialEase(5) { Mode = EasingMode.In });
            hw.Fan.TurnOn();
            //hw.FloorIndicator.StartFlicker();
            //elevatorEffectsPlayer.Play();

            hw.Fan.TurnOff();
            hw.HellLights.TurnOn();
            hw.CarriageDoor.Open();
            hw.Chandelier.TurnOff();

            hw.CarriageDoor.Close()
                .WaitOne();
        }

        /// <summary>
        /// Do not pass GO, do not collect $200.
        /// </summary>
        public static void GotoExit()
        {
            hw.CarriageDoor.Close();
            Goto(Location.BlackRockCity, 35);
            hw.CarriageDoor.Open();
        }

        private static void WaitAll(params WaitHandle[] handles)
        {
            WaitHandle.WaitAll(handles);
        }
        
        private static void Goto(Location destination, int duration, EasingFunction easing = null)
        {
            hw.BeginDestination(destination);
            hw.Turntable.Goto(destination);
            elevatorEffectsPlayer.Play(ElevatorEffect.Instance);

            var destFloor = destination.GetFloor();

            if(easing == null)
                easing = EasingFunction.ToDesitination;
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