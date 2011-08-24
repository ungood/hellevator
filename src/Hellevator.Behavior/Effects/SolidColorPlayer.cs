using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.Effects
{
    public class SolidColorPlayer
    {
        private readonly ILightStrip strip;

        public SolidColorPlayer(ILightStrip strip)
        {
            this.strip = strip;
        }

        public void Off()
        {
            Set(Colors.Black);
        }

        public void Set(Color color)
        {
            for(int i = 0; i < strip.NumLights; i++)
                strip.SetColor(i, color);
            strip.Update();
        }
    }
}