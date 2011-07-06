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

using System;
using System.Threading;
using System.Threading.Tasks;
using Hellevator.Behavior;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorTurntable : ViewModelBase, ITurntable
    {
        private float angle;

        private Location location;

        public float Angle
        {
            get { return angle; }
            set
            {
                if(value == angle)
                    return;

                angle = value;
                OnPropertyChanged("Angle");
            }
        }

        #region ITurntable Members

        public Location Location
        {
            get { return location; }
            set
            {
                if(value == location)
                    return;

                location = value;
                OnPropertyChanged("Location");
            }
        }

        public void Reset()
        {
            Location = Location.Entrance;
            Angle = 0;
        }

        public WaitHandle Goto(Location destination)
        {
            if(Location == Location.Unknown)
                Reset();

            var destAngle = GetDestinationAngle(destination);
            var delta = destAngle > Angle ? .5F : -0.5F;

            var resetEvent = new AutoResetEvent(false);
            Task.Factory.StartNew(() => {
                while(Math.Abs(Angle - destAngle) > 0.1F)
                {
                    Angle += delta;
                    Thread.Sleep(10);
                }
                Location = destination;
                resetEvent.Set();
            });

            return resetEvent;
        }

        private static int GetDestinationAngle(Location destination)
        {
            switch(destination)
            {
                case Location.Purgatory:
                    return 90;
                case Location.Hell:
                    return -90;
                case Location.BlackRockCity:
                    return 180;
                default:
                    return 0;
            }
        }

        #endregion
    }
}