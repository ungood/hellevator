using System;

namespace Hellevator.Behavior.Interface
{
    public interface IAudioZone
    {
        // TODO: Control/Fade volume
        // NOTE: Crossfade?
        // TODO: Chain songs in a folder (or related filename).
        // TODO: Wait for song to finish.
        void Play(string filename);
        void Stop();
    }
}
