#region License
// Copyright 2011 Jason Walker
// ungood@onetrue.name
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

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