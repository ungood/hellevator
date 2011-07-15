using System;
using System.Threading;

namespace Hellevator.Behavior.Interface
{
    public interface IDoor
    {
        WaitHandle Open();
        WaitHandle Close();
    }
}
