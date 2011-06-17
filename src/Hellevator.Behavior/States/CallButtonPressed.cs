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
            Hellevator.InsideZone.Play("CallButtonPressed_Inside");
            Hellevator.CarriageZone.Play("CallButtonPressed_Carriage");
            Hellevator.Chandelier.TurnOn();
            Hellevator.FloorIndicator.Floor = Floors.BlackRockCity;
            Hellevator.CarriageDoor.Open();

            Hellevator.PanelButton.Pressed += PanelButtonPressed;
        }

        private void PanelButtonPressed()
        {
            TransistionNext();
        }
    }
}
