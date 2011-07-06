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
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Components;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Interface
{
    public class ShiftRegisterLightStrip : ILightStrip
    {
        private readonly ShiftRegister shift;
        private readonly byte[] buffer;
        private readonly byte[] actual;

        private readonly object actualLock = new object();

        public ShiftRegisterLightStrip(FEZ_Pin.Digital dataPin, FEZ_Pin.Digital clockPin, FEZ_Pin.Digital latchPin, int numLights)
        {
            shift = new ShiftRegister((Cpu.Pin) dataPin, (Cpu.Pin) clockPin, (Cpu.Pin) latchPin);
            NumLights = numLights;
            buffer = new byte[numLights];
            actual = new byte[numLights];

            var thread = new Thread(Run);
            //thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        public int NumLights { get; private set; }
        
        public void SetColor(int light, Color color)
        {
            buffer[light] = color.Red;
        }

        public void Update()
        {
            lock(actualLock)
            {
                for(int i = 0; i < NumLights; i++)
                    actual[i] = buffer[i];
            }
        }

        private void Run()
        {
            var numBytes = NumLights / 8;
            if(NumLights % 8 != 0)
                numBytes++;
            var shiftData = new byte[numBytes];

            byte intensity = 0;
            while(true)
            {
                lock(actualLock)
                {
                    for(int light = 1; light <= NumLights; light++)
                    {
                        var lightIndex = NumLights - light;
                        var byteNum = lightIndex / 8;
                        var bitNum = lightIndex % 8;
                        shiftData[byteNum] = actual[lightIndex] > intensity
                            ? shiftData[byteNum].Set(bitNum)
                            : shiftData[byteNum].Clear(bitNum);
                    }
                }

                shift.ShiftOut(shiftData);

                intensity++;
                Thread.Sleep(0);
            }
        }
    }
}