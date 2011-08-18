using System;
using System.Threading;

namespace Hellevator.Behavior.Animations
{
    public class Animator
    {
        public delegate void SetFunc(double value, long ticks, bool final);

        public double InitialValue { get; set; }
        public double FinalValue { get; set; }
        public TimeSpan Length { get; set; }
        public EasingFunction EasingFunction { get; set; }
        public SetFunc Set { get; set; }

        public Animator()
        {
            InitialValue = 0;
            FinalValue = 1;
            Length = new TimeSpan(0, 0, 0, 1);
            EasingFunction = new LinearEase();
        }

        public void Animate()
        {
            var startTime = DateTime.Now;
            var lastTicks = startTime.Ticks;

            while(true)
            {
                var interval = DateTime.Now - startTime;
                if(interval > Length)
                    break;

                var ticks = interval.Ticks;
                var progress = (double) ticks / Length.Ticks;
                Set(Interpolate(InitialValue, FinalValue, progress, EasingFunction), ticks - lastTicks, false);
                lastTicks = ticks;
                Thread.Sleep(1);
            }

            Set(FinalValue, (DateTime.Now - startTime).Ticks - lastTicks, true);
        }

        public static double Interpolate(double initial, double final, double progress, EasingFunction easing)
        {
            var eased = easing.Ease(progress);
            return initial + ((final - initial) * eased);
        }
    }
}
