using System.Diagnostics;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorAudioZone : IAudioZone
    {
        public string Name { get; private set; }

        public SimulatorAudioZone(string name)
        {
            Name = name;
        }

        public void Play(string filename)
        {
            Debug.WriteLine("Playing {0} in {1}", filename, Name); // TODO: Actually play files.
        }

        public void Stop()
        {
            Debug.WriteLine("Stopping {1}", Name);
        }
    }
}