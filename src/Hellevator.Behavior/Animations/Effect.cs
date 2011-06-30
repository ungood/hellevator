using System;
using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.Animations
{
    public abstract class Effect
    {
        public abstract Color GetColor(int index, int numLights, double floor, long ticks);
    }
}
