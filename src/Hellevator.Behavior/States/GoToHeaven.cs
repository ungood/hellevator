using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.States
{
    public class GoToHeaven : State
    {
        protected override void Enter()
        {
            CarriageDoor.Close();
            Turntable.Goto(Destination.Hell);
            // TODO: Run Effects

        }

        protected override WaitHandle[] WaitHandles
        {
            get { return new[] {Turntable.RotateComplete}; }
        }
    }
}