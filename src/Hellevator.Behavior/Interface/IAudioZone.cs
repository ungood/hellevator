using System;
using System.Collections;
using System.Threading;

namespace Hellevator.Behavior.Interface
{
    public interface IAudioZone
    {
        WaitHandle Play(Playlist playlist);
        void Loop(Playlist playlist);
        WaitHandle Stop();
    }
}
