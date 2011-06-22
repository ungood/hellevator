namespace Hellevator.Behavior.Animations
{
    public class Color
    {
        public byte Red { get; private set; }
        public byte Green { get; private set; }
        public byte Blue { get; private set; }

        public Color(double red, double green, double blue)
        {
            Red = (byte) (red * 255);
            Green = (byte) (green * 255);
            Blue = (byte) (blue * 255);
        }

        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }

    public static class Colors
    {
        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color Red   = new Color(255, 0, 0);
    }
}