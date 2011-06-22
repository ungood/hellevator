namespace Hellevator.Behavior.Interface
{
    public interface IHellevator
    {
        IButton CallButton { get; }
        IButton PanelButton { get; }
        
        // Lights
        IRelay HellLights { get; }
        IRelay Chandelier { get; }
        ILightStrip Effects { get; }
        ILightStrip PanelLights { get; }

        // Music
        IAudioZone InsideZone { get; }
        IAudioZone CarriageZone { get; }

        // Action
        IDoor CarriageDoor { get; }
        ITurntable Turntable { get; }
        IRelay Fan { get; }
    }
}