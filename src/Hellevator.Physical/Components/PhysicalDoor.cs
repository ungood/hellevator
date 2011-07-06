using System;
using System.Threading;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;

namespace Hellevator.Physical.Components
{
    public class PhysicalDoor : IDoor
    {
        public void Open()
        {
            
        }

        public WaitHandle Close()
        {
            return null;
        }
    }
}
