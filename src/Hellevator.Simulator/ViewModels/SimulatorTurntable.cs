using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hellevator.Behavior;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorTurntable : ViewModelBase, ITurntable
    {
        private float angle;

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

        public SimulatorTurntable()
        {
            Task.Factory.StartNew(() => {
                while(true)
                {
                    Angle = Angle + 1F;
                    Thread.Sleep(100);
                }
            });
        }

        public void BeginGoto(Destinations destination)
        {
            // TODO
        }
    }
}
