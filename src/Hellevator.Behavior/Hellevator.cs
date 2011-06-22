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
            PanelPlayer = new AnimationPlayer(hardware.PanelLights);
            EffectPlayer = new AnimationPlayer(hardware.Effects);
        }

        public static double CurrentFloor { get; set; }

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

        public static AnimationPlayer PanelPlayer { get; private set; }

        public static AnimationPlayer EffectPlayer { get; private set; }

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
        
        #endregion
        
    }
}