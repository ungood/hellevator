using System;

namespace Hellevator.Behavior.Animations
{
    public class FloorIndicatorEffect : Effect
    {
        private readonly EasingFunction easeIn = new ExponentialEase() {
            Mode = EasingMode.In
        };

        private readonly EasingFunction easeOut = new ExponentialEase() {
            Mode = EasingMode.Out
        };
            

        public override Color GetColor(int index, int numLights, double floor, long time)
        {
            index = index + 1;
            var prev = (int)Math.Floor(floor);
            var ceil = (int)Math.Ceiling(floor);
            var intensity = floor - prev;
            
            if(index == prev && index == ceil)
                return Colors.Red;
            if(index == ceil)
                return new Color(Animator.Interpolate(0, 1, intensity, easeIn), 0, 0);
            if(index == prev)
                return new Color(Animator.Interpolate(1, 0, intensity, easeOut), 0, 0);

            return Colors.Black;
        }
    }
}
