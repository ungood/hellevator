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
                scenario.Run();
            }
        }
    }
}