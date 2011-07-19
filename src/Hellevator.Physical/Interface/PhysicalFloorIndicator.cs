using System;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;

namespace Hellevator.Physical.Interface
{
    public class PhysicalFloorIndicator : IFloorIndicator
    {
        public PhysicalFloorIndicator(FEZ_Pin.Digital dataPin, FEZ_Pin.Digital clockPin, FEZ_Pin.Digital latchPin)
        {

        }

        public int CurrentFloor
        {
            set { }
        }
    }
}
