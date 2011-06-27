using System;
using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior
{
    public static class Hellevator
    {
        public static IHellevator Hardware { get; private set; }

        public static void Initialize(IHellevator hardware)
        {
            Hardware = hardware;
            PanelPlayer = new EffectPlayer(hardware.PanelLights);
            EffectPlayer = new EffectPlayer(hardware.Effects);
        }

        public static double CurrentFloor { get; set; }

        private static readonly Effect PanelGotoEffect = new FloorIndicatorEffect();

        public static void Goto(Location destination, int msPerFloor, EasingFunction easing = null)
        {
            Turntable.Goto(destination);
            PanelPlayer.Play(PanelGotoEffect);
            EffectPlayer.Play(new RainbowEffect()); // TODO

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

            Turntable.FinishedGoing.WaitOne(); // Make sure the turntable has done its thing.
        }

        private static void SetCurrentFloor(double value)
        {
            CurrentFloor = value;
        }

        #region Inputs

        public static IButton CallButton
        {
            get { return Hardware.CallButton; }
        }

        public static IButton PanelButton
        {
            get { return Hardware.PanelButton; }
        }

        #endregion  

        #region Lights

        public static IRelay Chandelier
        {
            get { return Hardware.Chandelier; }
        }

        public static IRelay HellLights
        {
            get { return Hardware.HellLights; }
        }

        public static EffectPlayer PanelPlayer { get; private set; }

        public static EffectPlayer EffectPlayer { get; private set; }

        #endregion

        #region Sounds

        public static IAudioZone CarriageZone
        {
            get { return Hardware.CarriageZone; }
        }

        public static IAudioZone InsideZone
        {
            get { return Hardware.InsideZone; }
        }

        #endregion

        #region Action

        public static IDoor CarriageDoor
        {
            get { return Hardware.CarriageDoor; }
        }

        public static ITurntable Turntable
        {
            get { return Hardware.Turntable; }
        }

        public static IRelay Fan
        {
            get { return Hardware.Fan; }
        }
        
        #endregion
        
    }
}