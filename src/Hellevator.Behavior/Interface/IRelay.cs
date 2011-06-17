using System;
using Microsoft.SPOT;

namespace Hellevator.Behavior.Interface
{
    public interface IRelay
    {
        void TurnOn();
        void TurnOff();
    }
}
