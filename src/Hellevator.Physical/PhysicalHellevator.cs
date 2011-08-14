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

using System.IO.Ports;
using System.Threading;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Interface;
using Microsoft.SPOT;

namespace Hellevator.Physical
{
    public class PhysicalHellevator : IHellevator
    {
        public IButton CallButton { get; private set; }
        public IButton PanelButton { get; private set; }
        public IButton ModeButton { get; private set; }
        
        public IPatriotLight PatriotLight { get; private set; }
        public IFan Fan { get; private set; }
        public IRelay DriveWheel { get; private set; }
        public IRelay RopeLight { get; private set; }
        public IRelay SmokeMachine { get; private set; }

        public ILightStrip ElevatorEffects { get; private set; }
        public ILightStrip CeilingEffects { get; private set; }
        public IFloorIndicator FloorIndicator { get; private set; }
        
        public IAudioZone LobbyZone { get; private set; }
        public IAudioZone CarriageZone { get; private set; }
        public IAudioZone EffectsZone { get; private set; }
        
        public IDoor CarriageDoor { get; private set; }
        
        public Thread CreateThread(ThreadStart start)
        {
            return new Thread(start);
        }

        public void Display(string message)
        {
            Debug.Print(message);
        }

        public void BeginScenario(string name)
        {
            Debug.Print("Scenario: " + name);
        }

        public void BeginDestination(Location location)
        {
            Debug.Print("Location: " + location.ToString());
        }

        private readonly SerialPort audioSerial = new SerialPort("COM1", 115200);

        public PhysicalHellevator()
        {
            CallButton = new Button(FEZ_Pin.Interrupt.Di41);
            PanelButton = new Button(FEZ_Pin.Interrupt.Di43);
            ModeButton = new Button(FEZ_Pin.Interrupt.LDR);

            PatriotLight = new RelayPatriotLight(FEZ_Pin.Digital.Di52, FEZ_Pin.Digital.Di50, FEZ_Pin.Digital.Di48);
            Fan = new RelayFan(FEZ_Pin.Digital.Di28, FEZ_Pin.Digital.Di44);
            DriveWheel = new Relay(FEZ_Pin.Digital.Di46);
            RopeLight = new Relay(FEZ_Pin.Digital.Di26);
            SmokeMachine = new Relay(FEZ_Pin.Digital.Di24);

            ElevatorEffects = new SerialLedRope("COM2", 'a', 36);
            CeilingEffects = new SerialLedRope("COM4", 'b', 36);
            FloorIndicator = new ShiftFloorIndicator(FEZ_Pin.Digital.Di36, FEZ_Pin.Digital.Di34, FEZ_Pin.Digital.Di32);

            audioSerial.Open();
            EffectsZone = new SerialAudioZone(audioSerial, 0x01);
            CarriageZone = new SerialAudioZone(audioSerial, 0x02);
            LobbyZone    = new SerialAudioZone(audioSerial, 0x03);
            
            CarriageDoor = new Door();
        }
    }
}
