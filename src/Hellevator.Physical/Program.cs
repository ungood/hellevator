using System;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior;
using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Scenarios;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Math = System.Math;

namespace Hellevator.Physical
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("Starting");

            var input = new InputPort((Cpu.Pin) FEZ_Pin.Digital.LDR, false, Port.ResistorMode.PullUp);
            var output = new OutputPort((Cpu.Pin) FEZ_Pin.Digital.LED, false);
            //HellevatorScript.Run(new PhysicalHellevator());
            while(true)
            {
                output.Write(input.Read());
            }
        }

    }
}
