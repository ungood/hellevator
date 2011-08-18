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

using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Interface
{
    public class SpiFloorIndicator : IFloorIndicator
    {
        private readonly SPI spi;
        private readonly OutputPort latch;
        
        public SpiFloorIndicator(SPI.SPI_module module, FEZ_Pin.Digital latchPin)
        {
            var config = new SPI.Configuration(Cpu.Pin.GPIO_NONE, false, 0, 0, false, true, 1000, module);
            spi = new SPI(config);

            latch = new OutputPort((Cpu.Pin) latchPin, true);

            CurrentFloor = -1;
        }

        private int currentFloor;

        public int CurrentFloor
        {
            set
            {
                if(value == currentFloor)
                    return;
                currentFloor = value;

                if(value < 1 || value > 24)
                {
                    Write(0, 0, 0);
                    return;
                }
                
                value--;
                var buffer = new byte[3];
                buffer[value / 8] = (byte) (1 << (value % 8));
                Write(buffer);
            }
        }

        private void Write(params byte[] buffer)
        {
            latch.Write(false);
            spi.Write(buffer);
            latch.Write(true);
        }
    }
}
