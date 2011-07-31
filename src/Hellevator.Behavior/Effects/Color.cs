#region License
// Copyright 2011 Jason Walker
// ungood@onetrue.name
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

using System;

namespace Hellevator.Behavior.Effects
{
    public struct Color
    {
        public byte Red { get; private set; }
        public byte Green { get; private set; }
        public byte Blue { get; private set; }

        private double ScRed { get { return (double) Red / 255; } }
        private double ScGreen { get { return (double) Green / 255; } }
        private double ScBlue { get { return (double) Blue / 255; } }

        public Color(double red, double green, double blue)
            : this()
        {
            Red = (byte) (Clamp(red) * 255);
            Green = (byte) (Clamp(green) * 255);
            Blue = (byte) (Clamp(blue) * 255);
        }

        public Color(byte red, byte green, byte blue)
            : this()
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public static Color FromHSV(double h, double s, double v)
        {
            var hue60 = h / 60;
            var sector = (int)Math.Floor(hue60) % 6;
            var f = h / 60 - Math.Floor(h / 60);

            var p = v * (1 - s);
            var q = v * (1 - f * s);
            var t = v * (1 - (1 - f) * s);

            switch(sector)
            {
                case 0:
                    return new Color(v, t, p);
                case 1:
                    return new Color(q, v, p);
                case 2:
                    return new Color(p, v, t);
                case 3:
                    return new Color(p, q, v);
                case 4:
                    return new Color(t, p, v);
                default:
                    return new Color(v, p, q);
            }
        }

        public static Color operator+(Color left, Color right)
        {
            var red   = left.ScRed   + right.ScRed;
            var green = left.ScGreen + right.ScGreen;
            var blue  = left.ScBlue  + right.ScBlue;
            return new Color(red, green, blue);
        }

        public static Color operator*(Color left, Color right)
        {
            var red   = left.ScRed   * right.ScRed;
            var green = left.ScGreen * right.ScGreen;
            var blue = left.ScBlue * right.ScBlue;
            return new Color(red, green, blue);
        }

        public static Color operator*(Color left, double scalar)
        {
            var red = left.ScRed * scalar;
            var green = left.ScGreen * scalar;
            var blue = left.ScBlue * scalar;
            return new Color(red, green, blue);
        }

        private static double Clamp(double input)
        {
            return input < 0 ? 0 : (input > 1 ? 1 : input);
        }

        public static implicit operator ushort(Color color)
        {
            var r = color.Red >> 3;
            var g = color.Green >> 3;
            var b = color.Blue >> 3;

            // Bit pattern for color data: 1rrrrrgggggbbbbb
            return (ushort) (0x8000 | r << 10 | g << 5 | b);
        }

        public static explicit operator Color(ushort u)
        {
            var r = u >> 10 & 0x001f;
            var g = u >> 5 & 0x001f;
            var b = u & 0x001f;

            return new Color((byte) (r * 4), (byte) (g * 4), (byte) (b * 4));
        }
    }
}