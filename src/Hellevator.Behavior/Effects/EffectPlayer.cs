using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.Effects
{
    public class EffectPlayer
    {
        public ILightStrip Strip { get; private set; }

        private readonly double[] indexToRatio;
        private readonly int numLights;

        public EffectPlayer(ILightStrip strip)
        {
            Strip = strip;
            numLights = strip.NumLights;
            indexToRatio = new double[strip.NumLights];
            for(int i = 0; i < strip.NumLights; i++)
                indexToRatio[i] = 1 - ((double) i / strip.NumLights);
        }

        public const int TicksPerSecond = 25;
        public const int MilliPerTick = 15; // 115200 bps / (16*30)bits < 1000 / MilliPerTick

        private Timer timer;
        private long ticks;
        
        public void Play(Effect effect)
        {
            ticks = 0;
            if(timer != null)
            {
                timer.Dispose();
                timer = null;
            }
                
            timer = new Timer(TimerElapsed, effect, 0, MilliPerTick);
        }

        public void Off()
        {
            if(timer == null)
                return;

            timer.Dispose();
            timer = null;

            Thread.Sleep(4 * MilliPerTick);
            Strip.Reset();
        }

        private void TimerElapsed(object state)
        {
            if(timer == null)
                return;
            ticks++;
            
            var animation = (Effect) state;

            for(var light = 0; light < numLights; light++)
            {
                var color = animation.GetColor(indexToRatio[light], Hellevator.CurrentFloor, ticks * MilliPerTick);
                Strip.SetColor(numLights - light -1, color);
            }
            Strip.Update();
        }
    }
}
