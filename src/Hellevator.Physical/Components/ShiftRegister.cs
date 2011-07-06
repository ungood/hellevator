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

using System.Threading;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    public class ShiftRegister
    {
       

        private readonly OutputPort data;
        private readonly OutputPort clock;
        private readonly OutputPort latch;

        public ShiftRegister(Cpu.Pin dataPin, Cpu.Pin clockPin, Cpu.Pin latchPin)
        {
            data = new OutputPort(dataPin, false);
            clock = new OutputPort(clockPin, false);
            latch = new OutputPort(latchPin, true);
        }

        public void ShiftOut(params byte[] bytes)
        {
            latch.Write(true);

            foreach(var b in bytes)
            {
                for(var bit = 0; bit < 8; bit++)
                {
                    data.Write(b.IsSet(bit));

                    clock.Write(true);
                    Thread.Sleep(1);
                    clock.Write(false);
                }
            }

            latch.Write(false);
        }
    }
}
