using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.States
{
    public class GoToHeaven : State
    {
        protected override void Enter()
        {
            Hellevator.CarriageDoor.Close();
            Hellevator.Turntable.Goto(TurntableLocation.Hell);
            // TODO: Run Effects
            Hellevator.CurrentFloor = 7;
            //var thread = new Thread()
        }

        protected override void Wait()
        {
            while(Hellevator.CurrentFloor >= 1)
            {
                Hellevator.CurrentFloor -= 0.01;
                Thread.Sleep(10);
            }
            
            Hellevator.Turntable.FinishedGoing.WaitOne();
        }
    }
}