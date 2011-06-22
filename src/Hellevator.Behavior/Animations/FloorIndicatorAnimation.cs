using System;

namespace Hellevator.Behavior.Animations
{
    public class FloorIndicatorAnimation : Animation
    {
        public override Color GetColor(int index, double floor, long time)
        {
            index = index + 1;
            var prev = (int)Math.Floor(floor);
            var ceil = (int)Math.Ceiling(floor);
            var intensity = floor - prev;
            
            if(index == prev && index == ceil)
                return Colors.Red;
            if(index == ceil)
                return new Color(Easing(intensity), 0, 0);
            if(index == prev)
                return new Color(Easing(1.0 - intensity), 0, 0);

            return Colors.Black;
        }

        private double Easing(double t)
        {
            const double a = -10;
            var num = Math.Pow(Math.E, a * t) - 1;
            var denom = Math.Pow(Math.E, a) - 1;
            //var t2 = t * t;
            //var t3 = t2 * t;
            //var t4 = t3 * t;
            //var t5 = t4 * t;

            //return (6 * t5) + (-15 * t4) + (10 * t3);
            return num / denom;

        }
    }
}
