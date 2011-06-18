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
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class HellevatorSimulator : IHellevator
    {
        private readonly SimulatorButton callButton = new SimulatorButton();

        public IButton CallButton
        {
            get { return callButton; }
        }

        private readonly SimulatorButton panelButton = new SimulatorButton();

        public IButton PanelButton
        {
            get { return panelButton; }
        }

        private readonly SimulatorRelay fan = new SimulatorRelay();
        public IRelay Fan
        {
            get { return fan; }
        }

        private readonly SimulatorRelay chandelier = new SimulatorRelay();
        public IRelay Chandelier
        {
            get { return chandelier; }
        }

        private readonly SimulatorEffectPlayer effectPlayer = new SimulatorEffectPlayer();
        public IEffectPlayer EffectPlayer
        {
            get { return effectPlayer; }
        }

        private readonly SimulatorRelay hellLights = new SimulatorRelay();

        public IRelay HellLights
        {
            get { return hellLights; }
        }

        private readonly SimulatorFloorIndicator floorIndicator = new SimulatorFloorIndicator();
        public IFloorIndicator FloorIndicator
        {
            get { return floorIndicator; }
        }

        private readonly SimulatorAudioZone carriageZone = new SimulatorAudioZone("Carriage Zone");

        public IAudioZone CarriageZone
        {
            get { return carriageZone; }
        }

        private readonly SimulatorAudioZone insideZone = new SimulatorAudioZone("Inside Zone");

        public IAudioZone InsideZone
        {
            get { return insideZone; }
        }

        private readonly SimulatorDoor carriageDoor = new SimulatorDoor();
        public IDoor CarriageDoor
        {
            get { return carriageDoor; }
        }

        private readonly SimulatorTurntable turntable = new SimulatorTurntable();
        public ITurntable Turntable
        {
            get { return turntable; }
        }
    }
}