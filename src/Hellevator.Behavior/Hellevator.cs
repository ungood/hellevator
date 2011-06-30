using System;
using System.Threading;
using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Interface;
using Hellevator.Behavior.Scenarios;

namespace Hellevator.Behavior
{
    public static class Hellevator
    {
        #region Script

        public static IHellevator Hardware { get; private set; }
        private static readonly ScenarioLoop Loop = new ScenarioLoop {
            RandomScenario.Instance,
            HeavenHellScenario.Instance,
            HeavenScenario.Instance,
            PurgatoryScenario.Instance,
            HellScenario.Instance
        };

        public static void Run(IHellevator hardware)
        {
            Hardware = hardware;
            PanelPlayer = new EffectPlayer(hardware.PanelLights);
            EffectPlayer = new EffectPlayer(hardware.Effects);

            var buttons = new[] {ModeButton.Pressed, CallButton.Pressed};

            Reset();
            while(true)
            {
                Debug.Print(1, Loop.Current.Name);
                var buttonPressed = WaitHandle.WaitAny(buttons);
                switch(buttonPressed)
                {
                    case 0:
                        Loop.Next();
                        break;
                    case 1:
                        Loop.Current.Run();
                        Thread.Sleep(10 * 1000);
                        Reset();
                        break;
                }
            }
        }

        private static void Reset()
        {
            CarriageDoor.Close();
            Chandelier.TurnOff();
            CarriageZone.Stop();
            InsideZone.Stop();
            Fan.TurnOff();
            HellLights.TurnOff();
            EffectPlayer.Stop();
            PanelPlayer.Stop();
            CurrentFloor = Location.Entrance.GetFloor();
            Turntable.Reset();
        }

        #endregion

        #region Current Floor

        public static double CurrentFloor { get; set; }

        private static readonly Effect PanelGotoEffect = new FloorIndicatorEffect();

        private static readonly Effect elevatorMoveEffect = new MultiplierEffect(
            new RainbowEffect(), new ElevatorEffect());

        public static void Goto(Location destination, int msPerFloor, EasingFunction easing = null)
        {
            Turntable.Goto(destination);
            PanelPlayer.Play(PanelGotoEffect);
            EffectPlayer.Play(elevatorMoveEffect);

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

        #endregion

        #region Inputs

        public static IButton CallButton
        {
            get { return Hardware.CallButton; }
        }

        public static IButton PanelButton
        {
            get { return Hardware.PanelButton; }
        }

        public static IButton ModeButton
        {
            get { return Hardware.ModeButton; }
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

        public static ITextDisplay Debug
        {
            get { return Hardware.Debug; }
        }

        #endregion
        
    }
}