using System;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;

namespace Hellevator.Physical
{
    public class PhysicalInterface : IHellevatorInterface
    {
        public IButton CallButton
        {
            get { throw new NotImplementedException(); }
        }

        public IButton PanelButton
        {
            get { throw new NotImplementedException(); }
        }

        public IButton HellButton
        {
            get { throw new NotImplementedException(); }
        }

        public IRelay Chandelier
        {
            get { throw new NotImplementedException(); }
        }

        public IRelay HellLights
        {
            get { throw new NotImplementedException(); }
        }

        public IFloorIndicator FloorIndicator
        {
            get { throw new NotImplementedException(); }
        }

        public IAudioZone CarriageZone
        {
            get { throw new NotImplementedException(); }
        }

        public IAudioZone InsideZone
        {
            get { throw new NotImplementedException(); }
        }

        public IDoor CarriageDoor
        {
            get { throw new NotImplementedException(); }
        }
    }
}
