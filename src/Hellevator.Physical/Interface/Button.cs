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
using Hellevator.Behavior.Interface;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Interface
{
    public class Button : IButton
    {
        private readonly InterruptPort interrupt;
        private readonly AutoResetEvent interruptEvent = new AutoResetEvent(false);
        private DateTime lastPressed = DateTime.Now;

        public Button(FEZ_Pin.Interrupt pin)
        {
            interrupt = new InterruptPort((Cpu.Pin) pin, true, Port.ResistorMode.PullUp,
                Port.InterruptMode.InterruptEdgeHigh);
            interrupt.OnInterrupt += OnInterrupt;
            interrupt.EnableInterrupt();
        }

        private void OnInterrupt(uint data1, uint data2, DateTime time)
        {
            var interval = time - lastPressed;
            lastPressed = time;
            if(interval.Ticks < TimeSpan.TicksPerMillisecond * 250)
                return;

            interruptEvent.Set();
        }

        public event PressedEventHandler Pressed;
        
        public void Wait()
        {
            interruptEvent.Reset();
            interruptEvent.WaitOne();
        }
    }
}