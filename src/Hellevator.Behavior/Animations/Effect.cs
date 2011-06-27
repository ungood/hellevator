using System;
using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.Animations
{
    public abstract class Effect
    {
        public abstract Color GetColor(int index, double floor, long ticks);
    }
}
