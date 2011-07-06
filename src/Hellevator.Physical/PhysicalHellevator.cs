using System;
using System.Threading;
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
        public IButton ModeButton { get; private set; }
        public IRelay HellLights { get; private set; }
        public IRelay Chandelier { get; private set; }
        public ILightStrip VerticalChase { get; private set; }
        public ILightStrip PanelLights { get; private set; }
        public IAudioZone LobbyZone { get; private set; }
        public IAudioZone CarriageZone { get; private set; }
        public IAudioZone EffectsZone { get; private set; }
        public IDoor CarriageDoor { get; private set; }
        public ITurntable Turntable { get; private set; }
        public IRelay Fan { get; private set; }
        public ITextDisplay Debug { get; private set; }
        public Thread CreateThread(ThreadStart start)
        {
            throw new NotImplementedException();
        }

        public PhysicalHellevator()
        {
            CallButton = new PhysicalButton(FEZ_Pin.Interrupt.Di0);
            PanelButton = new PhysicalButton(FEZ_Pin.Interrupt.LDR);

            HellLights = new PhysicalRelay(FEZ_Pin.Digital.LED);
            Chandelier = new PhysicalRelay(FEZ_Pin.Digital.Di3);

            VerticalChase = new PhysicalLightStrip();
            PanelLights = new PhysicalLightStrip();

            CarriageZone = LobbyZone = new PhysicalAudioZone();

            CarriageDoor = new PhysicalDoor();

            Turntable = new PhysicalTurntable();

            Fan = new PhysicalRelay(FEZ_Pin.Digital.Di21);
        }
    }
}
