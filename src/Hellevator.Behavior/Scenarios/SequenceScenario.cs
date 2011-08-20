using System;

namespace Hellevator.Behavior.Scenarios
{
    public class SequenceScenario : Scenario
    {
        public static readonly SequenceScenario Instance = new SequenceScenario();

        private readonly ScenarioLoop loop = new ScenarioLoop {
            HellScenario.Instance,
            HeavenScenario.Instance,
            PurgatoryScenario.Instance,
            
        };

        public override string Name
        {
            get { return "SEQUENCE"; }
        }

        public override void Run()
        {
            loop.Current.Run();
            loop.Next();
        }
    }
}
