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

using Microsoft.SPOT;

namespace Hellevator.Behavior.Effects
{
    public class FadeToGoldEffect : Effect
    {
        private readonly WhiteNoiseEffect noise = new WhiteNoiseEffect();
        public int Seconds { get; private set; }
        
        public FadeToGoldEffect(int seconds)
        {
            Seconds = seconds;
        }
        
        public override Color GetColor(double light, double floor, long ms)
        {
            var progress = (double) ms / (Seconds * 250);
            if(progress >= 1.0)
                return Colors.Gold;

            var gold = Cache[(int) (progress * 100)];
            var intensity = RNG.Next(256) * (1 - progress);

            var red = gold.Red + intensity;
            var green = gold.Green + intensity;
            var blue = gold.Blue + intensity;

            red = red > 255 ? 255 : red;
            blue = blue > 255 ? 255 : blue;
            green = green > 255 ? 255 : green;
            return new Color((byte)red, (byte)green, (byte)blue);
            //return (Colors.Gold * progress) + (noise.GetColor(light, floor, ms) * (1 - progress));
        }

        private static readonly Color[] Cache = new Color[101];
        static FadeToGoldEffect()
        {
            for(int i = 0; i < 101; i++)
            {
                Cache[i] = Color.FromHSV(60, 1.0, (double) i / 100);
            }
        }
    }
}