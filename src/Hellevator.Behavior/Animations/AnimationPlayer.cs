using System;
using System.Threading;
using Hellevator.Behavior.Interface;

namespace Hellevator.Behavior.Animations
{
    public class AnimationPlayer
    {
        public ILightStrip Strip { get; private set; }

        public AnimationPlayer(ILightStrip strip)
        {
            Strip = strip;
        }

        private Timer timer;
        private long prevTicks;
        
        public void Play(Animation animation)
        {
            if(timer != null)
                Stop();

            prevTicks = DateTime.Now.Ticks;
            timer = new Timer(TimerElapsed, animation, 0, 10);
            //thread = new Thread(() => RunAnimation(animation));
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

            var animation = (Animation) state;
            
            for(var light = 0; light < Strip.NumLights; light++)
            {
                var color = animation.GetColor(light, Hellevator.CurrentFloor, ticks);
                Strip.SetColor(light, color);
            }
            Strip.Update();
        }
    }
}
