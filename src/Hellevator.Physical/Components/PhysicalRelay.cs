using System;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    public class PhysicalRelay : IRelay
    {
        private OutputPort port;

        public PhysicalRelay(FEZ_Pin.Digital pin)
        {
            port = new OutputPort((Cpu.Pin) pin, false);
        }

        public void TurnOn()
        {
            port.Write(true);
        }

        public void TurnOff()
        {
            port.Write(false);
        }
    }
}
