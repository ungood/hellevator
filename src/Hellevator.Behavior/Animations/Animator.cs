using System;
using System.Threading;

namespace Hellevator.Behavior.Animations
{
    public class Animator
    {
        public delegate void SetFunc(double value);

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
            EasingFunction = LinearEase.Identity;
        }

        public void Animate()
        {
            var startTime = DateTime.Now;

            while(true)
            {
                var interval = DateTime.Now - startTime;
                if(interval > Length)
                    break;

                var progress = (double) interval.Ticks / Length.Ticks;
                Set(Interpolate(InitialValue, FinalValue, progress, EasingFunction));
                Thread.Sleep(1);
            }

            Set(FinalValue);
        }

        public static double Interpolate(double initial, double final, double progress, EasingFunction easing)
        {
            var eased = easing.Ease(progress);
            return initial + ((final - initial) * eased);
        }
    }
}
