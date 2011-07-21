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
using Hellevator.Behavior;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class HellevatorSimulator : IHellevator
    {
        public IButton CallButton { get; private set; }
        public IButton PanelButton { get; private set; }
        public IButton ModeButton { get; private set; }
        
        public IRelay HellLights { get; private set; }
        public IRelay Chandelier { get; private set; }
        public ILightStrip ElevatorEffects { get; private set; }
        public IFloorIndicator FloorIndicator { get; private set; }
        
        public IAudioZone LobbyZone { get; private set; }
        public IAudioZone CarriageZone { get; private set; }
        public IAudioZone EffectsZone { get; private set; }
        
        public IDoor CarriageDoor { get; private set; }
        public IDoor MainDoor { get; private set; }
        public ITurntable Turntable { get; private set; }
        public IRelay Fan { get; private set; }
        public IRelay DriveWheel { get; private set; }
        
        public ITextDisplay Debug { get; private set; }
        
        public Thread CreateThread(ThreadStart start)
        {
            return new Thread(start);
        }

        public HellevatorSimulator()
        {
            CallButton = new SimulatorButton();
            PanelButton = new SimulatorButton();
            ModeButton = new SimulatorButton();

            HellLights = new SimulatorRelay();
            Chandelier = new SimulatorRelay();

            ElevatorEffects = new SimulatorLightStrip(50);
            FloorIndicator = new SimulatorFloorIndicator(24);

            LobbyZone = new SimulatorAudioZone("Inside Zone", -1);
            CarriageZone = new SimulatorAudioZone("Carriage Zone", 1);
            EffectsZone = new SimulatorAudioZone("Effects Zone", 0);

            CarriageDoor = new SimulatorDoor(true);
            MainDoor = new SimulatorDoor(false);
            DriveWheel = new SimulatorRelay();
            Turntable = new SimulatorTurntable();
            Fan = new SimulatorRelay();

            Debug = new SimulatorTextDisplay();
        }
    }
}