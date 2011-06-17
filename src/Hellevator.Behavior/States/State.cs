using System;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;

namespace Hellevator.Behavior.States
{
    public abstract class State
    {
        protected virtual void Enter() {}
        protected virtual void Loop(TimeSpan time) {}
        protected virtual void Exit() {}

        public void Run()
        {
            Enter();
            // TODO
        }

        public void TransistionNext() {}

        #region Inputs

        protected static IButton CallButton
        {
            get { return Hellevator.Current.CallButton; }
        }

        protected static IButton PanelButton
        {
            get { return Hellevator.Current.PanelButton; }
        }

        #endregion  

        #region Lights

        protected static IRelay Chandelier
        {
            get { return Hellevator.Current.Chandelier; }
        }

        protected static IFloorIndicator FloorIndicator
        {
            get { return Hellevator.Current.FloorIndicator; }
        }

        #endregion

        #region Sounds

        protected static IAudioZone CarriageZone
        {
            get { return Hellevator.Current.CarriageZone; }
        }

        protected static IAudioZone InsideZone
        {
            get { return Hellevator.Current.InsideZone; }
        }

        #endregion

        #region Action

        protected static IDoor CarriageDoor
        {
            get { return Hellevator.Current.CarriageDoor; }
        }
        
        #endregion
    }
}
