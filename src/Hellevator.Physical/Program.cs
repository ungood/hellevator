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
using shiftRegister;

namespace Hellevator.Physical
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("Starting");

            //var effect = new RainbowEffect();
            var rope = new LedRope(SPI.SPI_module.SPI2, 68);
           
            while(true)
            {
                rope.Update();
                Thread.Sleep(100);
            }
            
            //player.Play(effect);

            Thread.Sleep(Timeout.Infinite);
        }

        private static void ShiftRegisterTest(double value)
        {
            var module = SPI.SPI_module.SPI2;
            var latchPin = (Cpu.Pin) FEZ_Pin.Digital.Di52;
            var spiConfig = new SPI.Configuration(latchPin, true, 0, 0, false, true, 20000, module);
            var spi = new SPI(spiConfig);

            
            
            //shift.ShiftOut(0, 0, 0);
            //Thread.Sleep(1000);
            //shift.ShiftOut(16, 0, 0);

            //var led = new OutputPort((Cpu.Pin) FEZ_Pin.Digital.LED, true);

            var on = new byte[] {0, 1, 0};
            var off = new byte[] {0, 0, 0};
            var start = Utility.GetMachineTime();
            for(int i = 0; i < 10000; i++)
            {
                for(byte intensity = 0; intensity < 255; intensity++)
                {
                    if(intensity < 25)
                        spi.Write(on);
                    else
                        spi.Write(off);
                }
            }
            var end = Utility.GetMachineTime();

            Debug.Print((end - start).Milliseconds.ToString());
        }

    }
}
