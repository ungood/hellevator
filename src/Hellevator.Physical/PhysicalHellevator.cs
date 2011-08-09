using System;
using System.Threading;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Interface;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical
{
    public class PhysicalHellevator : IHellevator
    {
        public IButton CallButton { get; private set; }
        public IButton PanelButton { get; private set; }
        public IButton ModeButton { get; private set; }
        public IRelay HellLights { get; private set; }
        public IRelay Chandelier { get; private set; }
        public ILightStrip ElevatorEffects { get; private set; }
        public IFloorIndicator FloorIndicator { get; private set; }
        public ISequencedLight MoodLight { get; private set; }
        public IAudioZone LobbyZone { get; private set; }
        public IAudioZone CarriageZone { get; private set; }
        public IAudioZone EffectsZone { get; private set; }
        public IDoor CarriageDoor { get; private set; }
        public IDoor MainDoor { get; private set; }
        public ITurntable Turntable { get; private set; }
        public IRelay Fan { get; private set; }
        public IRelay DriveWheel { get; private set; }
        
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

        public PhysicalHellevator()
        {
            CallButton = PanelButton = new PhysicalButton(FEZ_Pin.Interrupt.LDR);
            //PanelButton = new PhysicalButton(FEZ_Pin.Interrupt.Di42);
            ModeButton = new PhysicalButton(FEZ_Pin.Interrupt.Di43);

            HellLights = new PhysicalRelay(FEZ_Pin.Digital.LED);
            Chandelier = new PhysicalRelay(FEZ_Pin.Digital.Di3);
            DriveWheel = new PhysicalRelay(FEZ_Pin.Digital.Di4);

            ElevatorEffects = new SerialLedRope("COM2", 'a', 40);
            FloorIndicator = new PhysicalFloorIndicator(FEZ_Pin.Digital.Di26, FEZ_Pin.Digital.Di27, FEZ_Pin.Digital.Di28);

            EffectsZone  = new PhysicalAudioZone(FEZ_Pin.Digital.Di10, FEZ_Pin.Digital.Di9, FEZ_Pin.Digital.Di8);
            CarriageZone = new PhysicalAudioZone(FEZ_Pin.Digital.Di7, FEZ_Pin.Digital.Di6, FEZ_Pin.Digital.Di5);
            LobbyZone    = new PhysicalAudioZone(FEZ_Pin.Digital.Di4, FEZ_Pin.Digital.Di3, FEZ_Pin.Digital.Di2);
            
            CarriageDoor = new PhysicalDoor();
            MainDoor = new PhysicalDoor();

            Turntable = new PhysicalTurntable();

            Fan = new PhysicalRelay(FEZ_Pin.Digital.Di29);
        }
    }
}
