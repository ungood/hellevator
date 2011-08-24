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

using System.IO;
using System.Threading;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.IO;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Audio
{
    public class Program
    {
        public static void Main()
        {
            var addressSetting = new InputPort((Cpu.Pin) FEZ_Pin.Digital.Di52, false, Port.ResistorMode.Disabled);
            var address = addressSetting.Read() ? 2 : 1;

            var storage = new PersistentStorage("SD");
            storage.MountFileSystem();

            var receiver = new SerialReceiver("COM1", 115200, (byte)address);
            var handler = new CommandHandler(FEZ_Pin.Digital.Di8);

            receiver.CommandReceived += handler.HandleCommand;
            
            while(true)
                Thread.Sleep(1000);
        }
    }
}
