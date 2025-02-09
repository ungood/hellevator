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
using Hellevator.Physical.Components;
using Hellevator.Physical.Interface;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Button = Hellevator.Physical.Interface.Button;

namespace Hellevator.Physical
{
    public class PhysicalHellevator : IHellevator
    {
        public IButton CallButton { get; private set; }
        public IButton PanelButton { get; private set; }
        public IButton ModeButton { get; private set; }
        
        public IPatriotLight PatriotLight { get; private set; }
        public IRelay Fan { get; private set; }
        public IRelay RopeLight { get; private set; }
        public IRelay DriveWheel { get; private set; }
        public IRelay SmokeMachine { get; private set; }

        public ILightStrip ElevatorEffects { get; private set; }
        public ILightStrip CeilingEffects { get; private set; }
        public IFloorIndicator FloorIndicator { get; private set; }
        
        public IAudioZone ExteriorZone { get; private set; }
        public IAudioZone InteriorZone { get; private set; }
        
        public IDoor CarriageDoor { get; private set; }
        
        public Thread CreateThread(ThreadStart start)
        {
            return new Thread(start);
        }

        public void DisplayScenario(string name)
        {
            lcd.Display(1, "MODE: " + name);
        }

        public void DisplayDestination(string name)
        {
            lcd.Display(2, name);
        }

        public void DisplayInstruction(string name)
        {
            lcd.Display(3, name);
        }

        private readonly AudioControllerCoordinator audioCoordinator = new AudioControllerCoordinator("COM1");
        private readonly ModernDeviceSerialLcd lcd = new ModernDeviceSerialLcd("COM3");

        public PhysicalHellevator()
        {
            CallButton = new Button(FEZ_Pin.Digital.Di43);
            PanelButton = new Button(FEZ_Pin.Digital.Di41);
            ModeButton = new Button(FEZ_Pin.Digital.Di21);

            PatriotLight = new RelayPatriotLight(FEZ_Pin.Digital.Di52, FEZ_Pin.Digital.Di50, FEZ_Pin.Digital.Di48);
            Fan = new Relay(FEZ_Pin.Digital.Di46);

            RopeLight = new Relay(FEZ_Pin.Digital.Di28);
            DriveWheel = new Relay(FEZ_Pin.Digital.Di26);
            SmokeMachine = new Relay(FEZ_Pin.Digital.Di24);

            ElevatorEffects = new SerialLedRope("COM2", 29);
            CeilingEffects = new SerialLedRope("COM4", 60);
            FloorIndicator = new SpiFloorIndicator(SPI.SPI_module.SPI2, FEZ_Pin.Digital.Di34);

            InteriorZone = new SerialAudioZone(audioCoordinator, 1, FEZ_Pin.Digital.Di5);
            ExteriorZone = new SerialAudioZone(audioCoordinator, 2, FEZ_Pin.Digital.Di10);
            
            CarriageDoor = new Door();

            lcd.Initialize();
        }
    }
}
