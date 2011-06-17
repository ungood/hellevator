using System;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;

namespace Hellevator.Behavior.States
{
    /// <summary>
    /// This state begins when the guest presses the call button, and ends
    /// when the guest enters the carriage and presses the panel button.
    /// </summary>
    /// <remarks>
    /// Assumptions:
    /// * Turntable is at "Heaven"
    /// * Outer doors have been opened.
    /// </remarks>
    public class CallButtonPressed : State
    {
        protected override void Enter()
        {
            InsideZone.Play("CallButtonPressed_Inside");
            CarriageZone.Play("CallButtonPressed_Carriage");
            Chandelier.TurnOn();
            FloorIndicator.Floor = Floors.BlackRockCity;
            CarriageDoor.Open();

            PanelButton.Pressed += PanelButtonPressed;
        }

        private void PanelButtonPressed()
        {
            TransistionNext();
        }
    }
}
