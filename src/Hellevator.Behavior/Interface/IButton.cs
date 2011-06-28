using System;
using System.Threading;

namespace Hellevator.Behavior.Interface
{
    public delegate void ClickedEventHandler();

    public interface IButton
    {
        //event ClickedEventHandler Clicked;
        WaitHandle Pressed { get; }
    }
}