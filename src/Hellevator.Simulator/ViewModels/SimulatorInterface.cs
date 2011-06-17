using System;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorInterface : IHellevatorInterface
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

        private readonly SimulatorRelay chandelier = new SimulatorRelay();

        public IRelay Chandelier
        {
            get { return chandelier; }
        }

        private readonly SimulatorRelay hellLights = new SimulatorRelay();

        public IRelay HellLights
        {
            get { return hellLights; }
        }

        public IFloorIndicator FloorIndicator
        {
            get { throw new NotImplementedException(); }
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

        public IDoor CarriageDoor
        {
            get { throw new NotImplementedException(); }
        }
    }
}