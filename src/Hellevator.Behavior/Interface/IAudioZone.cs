using System;
using Microsoft.SPOT;

namespace Hellevator.Behavior.Interface
{
    public interface IAudioZone
    {
        void Play(string filename);
        void Stop();
    }
}
