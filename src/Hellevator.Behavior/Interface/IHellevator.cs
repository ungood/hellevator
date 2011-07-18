using System.Threading;

namespace Hellevator.Behavior.Interface
{
    public interface IHellevator
    {
        // Buttons
        IButton CallButton { get; }
        IButton PanelButton { get; }
        IButton ModeButton { get; }
        
        // Lights
        IRelay HellLights { get; }
        IRelay Chandelier { get; }
        ILightStrip ElevatorEffects { get; }
        IFloorIndicator FloorIndicator { get; }

        // Music
        IAudioZone LobbyZone { get; }
        IAudioZone CarriageZone { get; }
        IAudioZone EffectsZone { get; }

        // Action
        IDoor CarriageDoor { get; }
        IDoor MainDoor { get; }
        ITurntable Turntable { get; }
        IRelay Fan { get; }
        IRelay DriveWheel { get; }

        // Utility
        ITextDisplay Debug { get; }
        Thread CreateThread(ThreadStart start);
    }
}