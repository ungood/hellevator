using System;
using Microsoft.SPOT;

namespace Hellevator.Behavior.States
{
    public class AttractState : State
    {
        protected override void Enter()
        {
            CarriageZone.Stop();
            InsideZone.Stop();
            FloorIndicator.TurnOff();
            // carriage doors close.
        }
    }
}
