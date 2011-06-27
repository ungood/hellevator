using System;
using System.Threading;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;

namespace Hellevator.Physical.Components
{
    public class PhysicalTurntable : ITurntable
    {
        public Location Location
        {
            get { return Location.BlackRockCity; }
        }

        public WaitHandle FinishedGoing
        {
            get { return null; }
        }

        public void Reset()
        {
            
        }

        public void Goto(Location destination)
        {
        }
    }
}
