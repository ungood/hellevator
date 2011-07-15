using System;
using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.Animations
{
    public class EffectPlayer
    {
        public ILightStrip Strip { get; private set; }

        public EffectPlayer(ILightStrip strip)
        {
            Strip = strip;
        }

        private Timer timer;
        private long prevTicks;
        
        public void Play(Effect effect)
        {
            if(timer != null)
                Stop();

            prevTicks = DateTime.Now.Ticks;
            timer = new Timer(TimerElapsed, effect, 0, 100);
            //thread = new Thread(() => RunAnimation(Effect));
            //thread.Priority = ThreadPriority.Lowest;
            //thread.Start();
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
                var color = animation.GetColor(light, Strip.NumLights, Script.CurrentFloor, ticks);
                Strip.SetColor(light, color);
            }
            Strip.Update();
        }
    }
}
