using System;
using Hellevator.Behavior.Interface;
using Hellevator.Behavior.Scenarios;
using Microsoft.SPOT;

namespace Hellevator.Behavior
{
    public static class Hellevator
    {
        private static IHellevatorInterface Interface { get; set; }

        public static void Run(IHellevatorInterface hal, Scenario scenario)
        {
            Interface = hal;
        }

        #region Inputs

        public static IButton CallButton
        {
            get { return Interface.CallButton; }
        }

        public static IButton PanelButton
        {
            get { return Interface.PanelButton; }
        }

        #endregion  

        #region Lights

        public static IRelay Chandelier
        {
            get { return Interface.Chandelier; }
        }

        public static IFloorIndicator FloorIndicator
        {
            get { return Interface.FloorIndicator; }
        }

        #endregion

        #region Sounds

        public static IAudioZone CarriageZone
        {
            get { return Interface.CarriageZone; }
        }

        public static IAudioZone InsideZone
        {
            get { return Interface.InsideZone; }
        }

        #endregion

        #region Action

        public static IDoor CarriageDoor
        {
            get { return Interface.CarriageDoor; }
        }
        
        #endregion
    }
}
