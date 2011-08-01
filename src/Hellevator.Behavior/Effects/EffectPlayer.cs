using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.Effects
{
    public class EffectPlayer
    {
        public ILightStrip Strip { get; private set; }

        private readonly double[] indexToRatio;

        public EffectPlayer(ILightStrip strip)
        {
            Strip = strip;
            indexToRatio = new double[strip.NumLights];
            for(int i = 0; i < strip.NumLights; i++)
                indexToRatio[i] = 1 - ((double) i / strip.NumLights);
        }

        public const int TicksPerSecond = 10;
        public const int MilliPerTick = 1000 / TicksPerSecond;

        private Timer timer;
        private long ticks;
        
        public void Play(Effect effect)
        {
            if(timer != null)
                Stop();

            timer = new Timer(TimerElapsed, effect, 0, MilliPerTick);
        }

        public void Stop()
        {
            if(timer == null)
                return;

            timer.Dispose();
            timer = null;
        }

        private void TimerElapsed(object state)
        {
            ticks++;
            
            var animation = (Effect) state;
            
            for(var light = 0; light < Strip.NumLights; light++)
            {
                var color = animation.GetColor(indexToRatio[light], Hellevator.CurrentFloor, ticks * MilliPerTick);
                Strip.SetColor(light, color);
            }
            Strip.Update();
        }
    }
}
