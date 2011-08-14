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
    public class Relay : IRelay
    {
        private readonly OutputPort port;

        public Relay(FEZ_Pin.Digital pin)
        {
            port = new OutputPort((Cpu.Pin) pin, true);
        }

        public void On()
        {
            port.Write(false);
        }

        public void Off()
        {
            port.Write(true);
        }
    }
}
