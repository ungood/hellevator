using System;

namespace Hellevator.Behavior.Interface
{
    public interface IFloorIndicator
    {
        float Floor { get; set; }
        void Flicker();
        void TurnOff();
    }
}
