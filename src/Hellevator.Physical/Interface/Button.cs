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
        private readonly InputPort interrupt;
        private readonly AutoResetEvent interruptEvent = new AutoResetEvent(false);
        private readonly Thread thread;

        public Button(FEZ_Pin.Digital pin, Port.ResistorMode mode = Port.ResistorMode.PullUp)
        {
            interrupt = new InputPort((Cpu.Pin) pin, false, mode);
            thread  = new Thread(Run);
            thread.Start();
        }

        private int count;
        
        private void Run()
        {
            while(true)
            {
                if(interrupt.Read() == false)
                {
                    count++;
                    if(count > 5)
                        OnPressed();
                }
                else
                {
                    count = 0;
                }
                Thread.Sleep(10);
            }
        }
        
        protected void OnPressed()
        {
            interruptEvent.Set();
            var handler = Pressed;
            if(handler != null)
                handler();
        }
        
        public event PressedEventHandler Pressed;
        
        public void Wait()
        {
            interruptEvent.Reset();
            interruptEvent.WaitOne();
        }
    }
}
