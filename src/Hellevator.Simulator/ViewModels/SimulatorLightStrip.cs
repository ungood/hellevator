using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Effects;
using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorLightStrip : ILightStrip
    {
        public ObservableCollection<LightViewModel> Lights { get; private set; }

        public SimulatorLightStrip(int numLights)
        {
            NumLights = numLights;

            Lights = new ObservableCollection<LightViewModel>();
            for(int i = 0; i < NumLights; i++)
                Lights.Add(new LightViewModel());
        }

        public int NumLights { get; private set; }

        public void SetColor(int light, Color color)
        {
            if(light < 0 || light >= NumLights)
                return;

            Lights[light].Color = color;
        }

        public void Update()
        {
            //Dispatcher.CurrentDispatcher.Invoke((Action)(() => {
                foreach(var light in Lights)
                    light.Update();
            //}));
        }

        public void Reset()
        {
            for(int i = 0; i < NumLights; i++)
                Lights[i].Color = Colors.Black;
        }
    }

    public class LightViewModel : ViewModelBase
    {
        public Color Color { get; set; }

        private int intensity;

        public int Intensity
        {
            get { return intensity; }
            set
            {
                if(value == intensity)
                    return;

                intensity = value;
                OnPropertyChanged("Intensity");
            }
        }

        public void Update()
        {
            OnPropertyChanged("Color");

            const int denom = 255 * 255;
            double max = Math.Max(Math.Max(Color.Red, Color.Blue), Color.Green);
            max = max * max;
            Intensity = (int) ((max / denom) * 20);
        }
    }
}