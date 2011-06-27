using System.Threading;
using Hellevator.Behavior.Interface;
using Hellevator.Behavior.Scenarios;

namespace Hellevator.Behavior
{
    public static class HellevatorScript
    {
        public static void Run(IHellevator hardware)
        {
            Hellevator.Initialize(hardware);
            var scenario = new PurgatoryScenario();

            while(true)
            {
                Reset();
                Hellevator.CallButton.Pressed.WaitOne();
                scenario.Run();
                Thread.Sleep(10000);
            }
        }

        private static void Reset()
        {
            Hellevator.CarriageDoor.Close();
            Hellevator.Chandelier.TurnOff();
            Hellevator.CarriageZone.Stop();
            Hellevator.InsideZone.Stop();
            Hellevator.Fan.TurnOff();
            Hellevator.HellLights.TurnOff();
            Hellevator.EffectPlayer.Stop();
            Hellevator.PanelPlayer.Stop();
            Hellevator.CurrentFloor = Location.Entrance.GetFloor();
            Hellevator.Turntable.Reset();
        }
    }
}