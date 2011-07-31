namespace Hellevator.Behavior.Effects
{
    public abstract class Effect
    {
        public abstract Color GetColor(double light, double floor, long ms);

        protected static double CalcPosition(double light, double floor)
        {
            return floor + light + 0.5;
        }
    }
}
