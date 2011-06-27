using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorDoor : ViewModelBase, IDoor
    {
        private bool isOpen;

        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                if(value == isOpen)
                    return;

                isOpen = value;
                OnPropertyChanged("IsOpen");
            }
        }   

        public void Open()
        {
            IsOpen = true;
        }

        public void Close()
        {
            IsOpen = false;
        }
    }
}
