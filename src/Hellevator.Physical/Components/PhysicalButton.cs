using System;
using System.Threading;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    public class PhysicalButton : IButton
    {
        private readonly InterruptPort interrupt;
        private AutoResetEvent pressed = new AutoResetEvent(false);

        public PhysicalButton(FEZ_Pin.Interrupt pin)
        {
            interrupt = new InterruptPort((Cpu.Pin)pin, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);
            interrupt.OnInterrupt += OnInterrupt;
            interrupt.EnableInterrupt();
        }

        private void OnInterrupt(uint data1, uint data2, DateTime time)
        {
            pressed.Set();
        }

        public WaitHandle Pressed
        {
            get { return pressed; }
        }
    }
}
