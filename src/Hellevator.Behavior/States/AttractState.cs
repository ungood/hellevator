using System;
using Microsoft.SPOT;

namespace Hellevator.Behavior.States
{
    public class AttractState : State
    {
        protected override void Enter()
        {
            Hellevator.CarriageZone.Stop();
            Hellevator.InsideZone.Stop();
            Hellevator.FloorIndicator.TurnOff();
            // carriage doors close.
        }
    }
}
