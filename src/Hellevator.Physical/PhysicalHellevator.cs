using System;
using GHIElectronics.NETMF.FEZ;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Components;
using Microsoft.SPOT;

namespace Hellevator.Physical
{
    public class PhysicalHellevator : IHellevator
    {
        public IButton CallButton { get; private set; }
        public IButton PanelButton { get; private set; }
        public IRelay HellLights { get; private set; }
        public IRelay Chandelier { get; private set; }
        public ILightStrip Effects { get; private set; }
        public ILightStrip PanelLights { get; private set; }
        public IAudioZone InsideZone { get; private set; }
        public IAudioZone CarriageZone { get; private set; }
        public IDoor CarriageDoor { get; private set; }
        public ITurntable Turntable { get; private set; }
        public IRelay Fan { get; private set; }

        public PhysicalHellevator()
        {
            CallButton = new PhysicalButton(FEZ_Pin.Interrupt.Di0);
            PanelButton = new PhysicalButton(FEZ_Pin.Interrupt.LDR);

            HellLights = new PhysicalRelay(FEZ_Pin.Digital.LED);
            Chandelier = new PhysicalRelay(FEZ_Pin.Digital.Di3);

            Effects = new PhysicalLightStrip();
            PanelLights = new PhysicalLightStrip();

            CarriageZone = InsideZone = new PhysicalAudioZone();

            CarriageDoor = new PhysicalDoor();

            Turntable = new PhysicalTurntable();

            Fan = new PhysicalRelay(FEZ_Pin.Digital.Di21);
        }
    }
}
