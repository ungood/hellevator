using System;
using System.Threading;

namespace Hellevator.Behavior.Interface
{
    public delegate void PressedEventHandler();

    public interface IButton
    {
        event PressedEventHandler Pressed;
        WaitHandle WaitHandle { get; }
    }
}