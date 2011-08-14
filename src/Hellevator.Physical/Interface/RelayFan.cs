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

namespace Hellevator.Physical.Interface
{
    public class RelayFan : IFan
    {
        private readonly Relay high;
        private readonly Relay low;

        public RelayFan(FEZ_Pin.Digital highPin, FEZ_Pin.Digital lowPin)
        {
            high = new Relay(highPin);
            low = new Relay(lowPin);
        }

        public void High()
        {
            low.Off();
            high.On();
        }

        public void Low()
        {
            high.Off();
            low.On();
        }

        public void Off()
        {
            high.Off();
            low.Off();
        }
    }
}
