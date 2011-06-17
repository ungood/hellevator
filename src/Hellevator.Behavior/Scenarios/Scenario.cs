using System;
using System.Threading;
using Hellevator.Behavior.Interface;
using Hellevator.Behavior.States;
using Microsoft.SPOT;

namespace Hellevator.Behavior.Scenarios
{
    public abstract class Scenario
    {
        public void Run(IHellevator hellevator)
        {
            Hellevator.Current = hellevator;

            Thread.Sleep(2000);
            new CallButtonPressed().Run();
        }
    }
}
