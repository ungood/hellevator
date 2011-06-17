using System;
using Microsoft.SPOT;

namespace Hellevator.Behavior.States
{
    public abstract class State
    {
        protected virtual void Enter() {}
        protected virtual void Loop(TimeSpan time) {}
        protected virtual void Exit() {}

        public void TransistionNext() {}
    }
}
