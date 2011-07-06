#region License
// Copyright 2011 Jason Walker
// ungood@onetrue.name
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

using System;
using System.Threading;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Animations;
using Hellevator.Physical.Components;
using Hellevator.Physical.Interface;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("Starting");

            //var input = new InputPort((Cpu.Pin) FEZ_Pin.Digital.LDR, false, Port.ResistorMode.PullUp);
            //var output = new OutputPort((Cpu.Pin) FEZ_Pin.Digital.LED, false);
            ////HellevatorScript.Run(new PhysicalHellevator());
            //while(true)
            //{
            //    output.Write(input.Read());
            //}

            //var shift = new ShiftRegister(
            //    (Cpu.Pin) FEZ_Pin.Digital.Di8,
            //    (Cpu.Pin) FEZ_Pin.Digital.Di9,
            //    (Cpu.Pin) FEZ_Pin.Digital.Di10);

            //while(true)
            //{
            //    for(byte intensity = 0; intensity <= 255; intensity++)
            //    {
            //        if(intensity > 125)
            //            shift.ShiftOut(0, 0, 0);
            //        else
            //            shift.ShiftOut(0, 0, 1);
            //    }
            //}

            var lightStrip = new ShiftRegisterLightStrip(FEZ_Pin.Digital.Di8, FEZ_Pin.Digital.Di9, FEZ_Pin.Digital.Di10, 24);
            var player = new EffectPlayer(lightStrip);
            var effect = new FloorIndicatorEffect();
            player.Play(effect);

            //while(true)
                //{
                //    Hellevator.Behavior.Hellevator.CurrentFloor = 7.0;
                //    Thread.Sleep(1000);
                //    Hellevator.Behavior.Hellevator.CurrentFloor = 6.5;
                //    Thread.Sleep(1000);
                //    Hellevator.Behavior.Hellevator.CurrentFloor = 23;
                //    Thread.Sleep(1000);
                //}

                while(true)
                {
                    var animator = new Animator {
                        InitialValue = 1.0,
                        FinalValue = 24.0,
                        EasingFunction = new LinearEase(),
                        Length = new TimeSpan(0, 0, 0, 30),
                        Set = SetCurrentFloor
                    };
                    animator.Animate();
                }
                Hellevator.Behavior.Hellevator.CurrentFloor = 6.5;
        }

        private static void SetCurrentFloor(double value)
        {
            Hellevator.Behavior.Hellevator.CurrentFloor = value;
        }

    }
}
