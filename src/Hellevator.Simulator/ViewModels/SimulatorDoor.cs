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

using System.Threading;
using System.Threading.Tasks;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorDoor : ViewModelBase, IDoor
    {
        public string Name { get; set; }
        private const int OpenCloseDelay = 500;
        private bool isClosed;

        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                if(value == isClosed)
                    return;

                isClosed = value;
                OnPropertyChanged("IsClosed");
            }
        }

        public SimulatorDoor(string name)
        {
            Name = name;
        }

        private WaitHandle OpenClose(bool isClosed)
        {
            var evt = new ManualResetEvent(false);
            Task.Factory.StartNew(() => {
                IsClosed = isClosed;
                Thread.Sleep(OpenCloseDelay);
                evt.Set();
            });
            return evt;
        }

        public WaitHandle Open()
        {
            Stopwatch.Print("Open {0}", Name);
            return OpenClose(false);
        }

        public WaitHandle Close()
        {
            Stopwatch.Print("Close {0}", Name);
            return OpenClose(true);
        }
    }
}