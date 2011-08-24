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
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Interface
{
    public class SpiFloorIndicator : IFloorIndicator
    {
        private readonly SPI spi;
        private readonly OutputPort latch;
        private readonly ExtendedTimer timer;
        
        public SpiFloorIndicator(SPI.SPI_module module, FEZ_Pin.Digital latchPin)
        {
            var config = new SPI.Configuration(Cpu.Pin.GPIO_NONE, false, 0, 0, false, true, 200, module);
            spi = new SPI(config);

            latch = new OutputPort((Cpu.Pin) latchPin, true);

            CurrentFloor = 1;

            timer = new ExtendedTimer(UpdateIndicator, null, DateTime.Now,
                TimeSpan.FromTicks(25 * TimeSpan.TicksPerMillisecond));
        }

        private int currentFloor;
        private readonly byte[] buffer = new byte[3];

        public int CurrentFloor
        {
            get { return currentFloor; }
            set
            {
                if(value == currentFloor)
                    return;
                currentFloor = value;

                while(value > 24)
                    value -= 24;
                while(value < 1)
                    value += 24;

                value = (24 - value);
                buffer[0] = buffer[1] = buffer[2] = 0;
                buffer[2- (value / 8)] = (byte) (1 << (value % 8));

                
            }
        }

        private void UpdateIndicator(object state)
        {
            latch.Write(false);
            spi.Write(buffer);
            latch.Write(true);
        }
    }
}
