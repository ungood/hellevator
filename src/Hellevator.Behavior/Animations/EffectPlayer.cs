using System;
using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.Animations
{
    public class EffectPlayer
    {
        public ILightStrip Strip { get; private set; }

        private double[] indexToRatio;

        public EffectPlayer(ILightStrip strip)
        {
            Strip = strip;
            indexToRatio = new double[strip.NumLights];
            for(int i = 0; i < strip.NumLights; i++)
                indexToRatio[i] = (double) i / strip.NumLights;
        }

        private Timer timer;
        private long prevTicks;
        
        public void Play(Effect effect)
        {
            if(timer != null)
                Stop();

            prevTicks = DateTime.Now.Ticks;
            timer = new Timer(TimerElapsed, effect, 0, 100);
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
            var currentTicks = DateTime.Now.Ticks;
            var ticks = currentTicks - prevTicks;
            prevTicks = currentTicks;

            var animation = (Effect) state;
            
            for(var light = 0; light < Strip.NumLights; light++)
            {
                var color = animation.GetColor(indexToRatio[light], Hellevator.CurrentFloor, ticks);
                Strip.SetColor(light, color);
            }
            Strip.Update();
        }
    }
}
