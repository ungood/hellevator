using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorFloorIndicator : ViewModelBase, IFloorIndicator
    {
        public ObservableCollection<Light> Lights { get; private set; }
        public int NumLights { get; private set; }
        private int currentFloor = 0;

        public SimulatorFloorIndicator(int numLights)
        {
            NumLights = numLights;
            Lights = new ObservableCollection<Light>();
            for(int i = 0; i < NumLights; i++)
                Lights.Add(new Light());
            CurrentFloor = 7;
        }

        public int CurrentFloor
        {
            set
            {
                if(value == currentFloor)
                    return;

                currentFloor = value;
                Stopwatch.Print("Floor {0}", value);

                foreach(var light in Lights)
                    light.IsOn = false;

                if(value < 1 || value > NumLights)
                    return;

                Lights[value - 1].IsOn = true;
            }
        }

        public class Light : ViewModelBase
        {
            private bool isOn;

            public bool IsOn
            {
                get { return isOn; }
                set
                {
                    if(value == isOn)
                        return;

                    isOn = value;
                    OnPropertyChanged("IsOn");
                }
            }
        }
    }
}
