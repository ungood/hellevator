using System.Threading;

namespace Hellevator.Behavior.Interface
{
    public interface IHellevator
    {
        // Buttons
        IButton CallButton { get; }
        IButton PanelButton { get; }
        IButton ModeButton { get; }

        // Relays
        IPatriotLight PatriotLight { get; }
        IRelay Fan { get; }
        IRelay DriveWheel { get; }
        IRelay RopeLight { get; }
        
        // Lights
        ILightStrip ElevatorEffects { get; }
        ILightStrip CeilingEffects { get; }
        IFloorIndicator FloorIndicator { get; }
        
        // Music
        IAudioZone LobbyZone { get; }
        IAudioZone CarriageZone { get; }
        IAudioZone EffectsZone { get; }

        // Action
        IDoor CarriageDoor { get; }
        
        // Utility
        Thread CreateThread(ThreadStart start);

        void DisplayScenario(string name);
        void DisplayDestination(string name);
        void DisplayInstruction(string name);
    }
}