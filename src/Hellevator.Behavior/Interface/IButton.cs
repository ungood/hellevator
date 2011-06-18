using System;
using System.Threading;

namespace Hellevator.Behavior.Interface
{
    public interface IButton
    {
        WaitHandle Pressed { get; }
    }
}