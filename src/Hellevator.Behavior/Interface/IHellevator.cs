using System;

namespace Hellevator.Behavior.Interface
{
    public interface IHellevator
    {
        // Inputs
        IButton CallButton { get; }
        IButton PanelButton { get; }
        
        // Lights
        IRelay HellLights { get; }
        IRelay Chandelier { get; }
        IEffectPlayer EffectPlayer { get; }
        IFloorIndicator FloorIndicator { get; }

        // Music
        IAudioZone InsideZone { get; }
        IAudioZone CarriageZone { get; }

        // Action
        IDoor CarriageDoor { get; }
        ITurntable Turntable { get; }
        IRelay Fan { get; }
    }
}
