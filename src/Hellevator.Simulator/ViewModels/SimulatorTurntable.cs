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
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorTurntable : ViewModelBase, ITurntable
    {
        private readonly AutoResetEvent finishedGoing = new AutoResetEvent(false);
        private float angle;

        private TurntableLocation location;

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

        public TurntableLocation Location
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

        public WaitHandle FinishedGoing
        {
            get { return finishedGoing; }
        }

        public void Reset()
        {
            Location = TurntableLocation.Entrance;
            Angle = 0;
        }

        public void Goto(TurntableLocation destination)
        {
            if(Location == TurntableLocation.Unknown)
                Reset();

            var destAngle = (int) destination * 90;
            var delta = destAngle > Angle ? .5F : -0.5F;
            Task.Factory.StartNew(() => {
                while(Math.Abs(Angle - destAngle) > 0.1F)
                {
                    Angle += delta;
                    Thread.Sleep(10);
                }
                Location = destination;
                finishedGoing.Set();
            });
        }

        public void Rotate(int degrees)
        {
            Angle += degrees;

            finishedGoing.Set();
        }

        #endregion
    }
}