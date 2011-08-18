using System;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Components;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Interface
{
    public class ShiftFloorIndicator : IFloorIndicator
    {
        private readonly ShiftRegister shift;

        public ShiftFloorIndicator(FEZ_Pin.Digital dataPin, FEZ_Pin.Digital clockPin, FEZ_Pin.Digital latchPin)
        {
            shift = new ShiftRegister((Cpu.Pin)dataPin, (Cpu.Pin)clockPin, (Cpu.Pin)latchPin);
            CurrentFloor = -1;
        }

        private int currentFloor = 0;
        public int CurrentFloor
        {
            set
            {
                if(currentFloor == value)
                    return;
                currentFloor = value;

                while(value > 24)
                    value -= 24;
                if(value < 1)
                {
                    shift.ShiftOut(new byte[3]);
                    return;
                }

                value--;
                var data = new byte[3];
                data[value / 8] = (byte) (1 << (value % 8));
                shift.ShiftOut(data);
            }
        }
    }
}
