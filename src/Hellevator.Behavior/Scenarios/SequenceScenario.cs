using System;

namespace Hellevator.Behavior.Scenarios
{
    public class SequenceScenario : Scenario
    {
        public static readonly SequenceScenario Instance = new SequenceScenario();

        private readonly ScenarioLoop loop = new ScenarioLoop {
            HeavenScenario.Instance,
            PurgatoryScenario.Instance,
            HellScenario.Instance
        };

        public override string Name
        {
            get { return "SEQ: " + loop.Current.Name; }
        }

        public override void Run()
        {
            loop.Current.Run();
            loop.Next();
        }
    }
}
