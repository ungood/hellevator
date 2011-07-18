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
using Hellevator.Behavior.Interface;

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
        }

        public static void Display(int line, string message)
        {
            hw.Debug.Print(line, message);
        }

        /// <summary>
        /// Oh SHIT.
        /// </summary>
        public static void EmergencyStop()
        {
            
        }

        /// <summary>
        /// Begin attract mode.
        /// </summary>
        public static void Reset(bool firstTime)
        {
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
            CurrentFloor = Location.Entrance.GetFloor();
            hw.Chandelier.TurnOn();
            hw.CarriageZone.Loop(Playlist.ElevatorMusic);
            WaitAll(
                hw.CarriageDoor.Open(),
                hw.LobbyZone.Play(Playlist.WarmupSounds));
                
            // Open main doors to accept guest
            hw.LobbyZone.Loop(Playlist.IdleSounds);
            hw.MainDoor.Open()
                .WaitOne();
            
            // Wait for guest to press the panel button.
            hw.PanelButton.Wait();
            hw.EffectsZone.Play(Playlist.Ding);
            
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
            hw.Debug.Print(2, "TO: HEAVEN");

            hw.CarriageDoor.Close();
            Goto(Location.Heaven, 1500);
            hw.CarriageZone.Stop();
            hw.CarriageDoor.Open();

            hw.PanelButton.Wait();
        }

        /// <summary>
        /// Send the guest to land of the half-damned.
        /// </summary>
        public static void GotoPurgatory()
        {
            hw.Debug.Print(2, "TO: PURGATORY");

            hw.CarriageDoor.Close();
            Goto(Location.Purgatory, 1000);
            hw.CarriageDoor.Open();

            Thread.Sleep(5000);
        }

        /// <summary>
        /// TONIGHT WE DINE IN HELL!
        /// </summary>
        public static void GotoHell()
        {
            hw.Debug.Print(2, "TO: HELL");

            hw.CarriageZone.Stop();
            hw.LobbyZone.Stop();

            hw.CarriageDoor.Close();
            hw.Fan.TurnOn();
            Goto(Location.Hell, 300, new ExponentialEase(5) { Mode = EasingMode.In });
            hw.Fan.TurnOff();
            hw.HellLights.TurnOn();
            hw.CarriageDoor.Open();
            hw.Chandelier.TurnOff();

            hw.PanelButton.Wait();
        }

        /// <summary>
        /// Do not pass GO, do not collect $200.
        /// </summary>
        public static void GotoExit()
        {
            hw.Debug.Print(2, "TO: EXIT");

            hw.CarriageDoor.Close();
            Goto(Location.BlackRockCity, 1500);
            hw.CarriageDoor.Open();
        }

        private static void WaitAll(params WaitHandle[] handles)
        {
            WaitHandle.WaitAll(handles);
        }
        
        private static readonly Effect elevatorMoveEffect = new ElevatorEffect();

        private static void Goto(Location destination, int msPerFloor, EasingFunction easing = null)
        {
            hw.Turntable.Goto(destination);
            elevatorEffectsPlayer.Play(elevatorMoveEffect);

            var destFloor = destination.GetFloor();

            if(easing == null)
                easing = LinearEase.Identity;
            var deltaFloors = Math.Abs((int) (destFloor - CurrentFloor));
            var length = new TimeSpan(0, 0, 0, 0, msPerFloor * deltaFloors);

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
    }
}