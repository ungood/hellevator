using System;
using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.States
{
    public abstract class State
    {
        protected virtual void Enter() {}
        protected virtual WaitHandle[] WaitHandles
        {
            get { return new WaitHandle[0]; }
        }
        
        protected virtual void Exit() {}

        public void Run()
        {
            Enter();
            WaitHandle.WaitAll(WaitHandles);
            Exit();
        }

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

        protected static IRelay HellLights
        {
            get { return Hellevator.Current.HellLights; }
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

        protected static ITurntable Turntable
        {
            get { return Hellevator.Current.Turntable; }
        }
        
        #endregion
    }
}


