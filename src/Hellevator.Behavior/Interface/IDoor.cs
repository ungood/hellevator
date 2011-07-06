using System;
using System.Threading;

namespace Hellevator.Behavior.Interface
{
    public interface IDoor
    {
        void Open();
        WaitHandle Close();
    }
}
