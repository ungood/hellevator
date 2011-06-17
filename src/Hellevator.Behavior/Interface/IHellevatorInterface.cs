using System;
using Microsoft.SPOT;

namespace Hellevator.Behavior.Interface
{
    public interface IHellevatorInterface
    {
        // Inputs
        IButton CallButton { get; }
        IButton PanelButton { get; }

        // Lights
        IRelay Chandelier { get; }
        IRelay HellLights { get; }
        IFloorIndicator FloorIndicator { get; }

        // Sounds
        IAudioZone CarriageZone { get; }
        IAudioZone InsideZone { get; }
        
        // Action
        IDoor CarriageDoor { get; }

        // Magic
    }
}
