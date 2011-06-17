using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorRelay : ViewModelBase, IRelay
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

        public void TurnOn()
        {
            IsOn = true;
        }

        public void TurnOff()
        {
            IsOn = false;
        }
    }
}