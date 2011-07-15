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

using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior
{
    internal static class Hellevator
    {
        private static IHellevator Hardware;

        private static EffectPlayer VerticalChasePlayer;
        private static EffectPlayer PanelPlayer;

        public static void Initialize(IHellevator hardware)
        {
            Hardware = hardware;
            PanelPlayer = new EffectPlayer(hardware.PanelLights);
            VerticalChasePlayer = new EffectPlayer(hardware.VerticalChase);
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
            Hardware.CarriageDoor.Close();
            Hardware.Chandelier.TurnOff();
            Hardware.CarriageZone.Stop();
            Hardware.LobbyZone.Stop();
            Hardware.Fan.TurnOff();
            Hardware.HellLights.TurnOff();
            
            VerticalChasePlayer.Stop();
            PanelPlayer.Stop();
            CurrentFloor = Location.Entrance.GetFloor();
            
            Hardware.Turntable.Reset();
        }

        /// <summary>
        /// Wait for the guest to get in the damned carriage.
        /// </summary>
        public static void AcceptGuest()
        {
            CurrentFloor = Location.Entrance.GetFloor();
            Hardware.Chandelier.TurnOn();
            Hardware.CarriageZone.Loop(Playlist.ElevatorMusic);
            WaitAll(
                Hardware.CarriageDoor.Open(),
                Hardware.LobbyZone.Play(Playlist.WarmupSounds));
                
            // Open main doors to accept guest
            Hardware.LobbyZone.Loop(Playlist.IdleSounds);
            Hardware.MainDoor.Open()
                .WaitOne();
            
            // Wait for guest to press the panel button.
            Hardware.PanelButton.WaitHandle
                .WaitOne();
            Hardware.EffectsZone.Play(Playlist.Ding);
            
            // Wait for the doors to close before getting underway.
            Hardware.CarriageDoor.Close()
                .WaitOne();
            Hardware.MainDoor.Close()
                .WaitOne();

            Hardware.CarriageZone.Stop();
            Hardware.EffectsZone.Play(Playlist.WelcomeToHellevator)
                .WaitOne();
        }

        /// <summary>
        /// Take the guest on the stairway to heaven.
        /// </summary>
        public static void GotoHeaven()
        {
            Hardware.Debug.Print(2, "TO: HEAVEN");

            Hardware.CarriageDoor.Close();
            Hardware.CarriageZone.Play("TheGirl");
            Hardware.Goto(Location.Heaven, 1500);
            Hardware.CarriageZone.Stop();
            Hardware.CarriageDoor.Open();
        }

        /// <summary>
        /// Send the guest to land of the half-damned.
        /// </summary>
        public static void GotoPurgatory()
        {
            Hardware.Debug.Print(2, "TO: PURGATORY");

            Hardware.CarriageDoor.Close();
            Hardware.CarriageZone.Play("Kalimba");
            Hardware.LobbyZone.Play("Sleep Away");
            Hardware.Goto(Location.Purgatory, 1000);
            Hardware.CarriageDoor.Open();
        }

        /// <summary>
        /// TONIGHT WE DINE IN HELL!
        /// </summary>
        public static void GotoHell()
        {
            Hardware.Debug.Print(2, "TO: HELL");

            Hardware.CarriageZone.Stop();
            Hardware.LobbyZone.Stop();

            Hardware.CarriageDoor.Close();
            Hardware.Fan.TurnOn();
            Hardware.Goto(Location.Hell, 300, new ExponentialEase(5) { Mode = EasingMode.In });
            Hardware.Fan.TurnOff();
            Hardware.HellLights.TurnOn();
            Hardware.CarriageDoor.Open();
            Hardware.Chandelier.TurnOff();
        }

        /// <summary>
        /// Do not pass GO, do not collect $200.
        /// </summary>
        public static void GotoExit()
        {
            Hardware.Debug.Print(2, "TO: EXIT");

            Hardware.CarriageDoor.Close();
            Hardware.Goto(Location.BlackRockCity, 1500);
            Hardware.CarriageDoor.Open();
        }

        private static void WaitAll(params WaitHandle[] handles)
        {
            WaitHandle.WaitAll(handles);
        }

         public static double CurrentFloor { get; set; }

        private static readonly Effect PanelGotoEffect = new FloorIndicatorEffect();

        private static readonly Effect elevatorMoveEffect = new MultiplierEffect(
            new RainbowEffect(), new ElevatorEffect());

        public static void Goto(Location destination, int msPerFloor, EasingFunction easing = null)
        {
            Turntable.Goto(destination);
            PanelPlayer.Play(PanelGotoEffect);
            VerticalChasePlayer.Play(elevatorMoveEffect);

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
                Set = SetCurrentFloor
            };
            animator.Animate();
        }

        private static void SetCurrentFloor(double value)
        {
            CurrentFloor = value;
        }
    }
}