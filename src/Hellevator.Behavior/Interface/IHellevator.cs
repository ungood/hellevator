using System;
using Microsoft.SPOT;

namespace Hellevator.Behavior.Interface
{
    public interface IHellevator
    {
        // Outside
        IButton CallButton { get; }

        // Inside / Hell
        IAudioZone InsideZone { get; }
        IRelay HellLights { get; }

        // Carriage
        IDoor CarriageDoor { get; }
        ITurntable CarriageTurntable { get; }
        IRelay Fan { get; }
        IRelay Chandelier { get; }
        IEffectPlayer FloorEffects { get; }
        IAudioZone CarriageZone { get; }

        // Panel
        IButton PanelButton { get; }
        IFloorIndicator FloorIndicator { get; }
    }
}
