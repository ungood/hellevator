using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.States
{
    public class GoToHeaven : State
    {
        protected override void Enter()
        {
            CarriageDoor.Close();
            Turntable.Goto(TurntableLocation.Hell);
            // TODO: Run Effects
            FloorIndicator.Floor = 6.7;
        }

        protected override WaitHandle[] WaitHandles
        {
            get { return new[] {Turntable.FinishedGoing}; }
        }
    }
}