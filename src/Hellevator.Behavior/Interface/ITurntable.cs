using System;
using Microsoft.SPOT;

namespace Hellevator.Behavior.Interface
{
    public interface ITurntable
    {
        void BeginGoto(Destinations destination);
    }
}
