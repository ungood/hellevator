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
    public class RelayPatriotLight : IPatriotLight
    {
        private readonly Relay red;
        private readonly Relay white;
        private readonly Relay blue;

        public RelayPatriotLight(FEZ_Pin.Digital redPin, FEZ_Pin.Digital whitePin, FEZ_Pin.Digital bluePin)
        {
            red = new Relay(redPin);
            white = new Relay(whitePin);
            blue = new Relay(bluePin);
        }

        public void Red()
        {
            Off();
            red.On();
        }

        public void White()
        {
            Off();
            white.On();
        }

        public void Blue()
        {
            Off();
            blue.On();
        }

        public void Off()
        {
            red.Off();
            white.Off();
            blue.Off();
        }
    }
}
