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

namespace Hellevator.Behavior.Effects
{
    public class HeavenEffect : Effect
    {
        private readonly RainbowEffect rainbow = new RainbowEffect(0.5, 0.5);

        public override Color GetColor(double light, double floor, long ms)
        {
            var position = CalcPosition(light, floor);

            if(position < 24)
                return Colors.Black;

            // white -> blue -> black;
            if(24 <= position && position < 72)
            {
                var s = (position - 24) / 48;
                var v = 1 - s;
                return Color.FromHSV(240, s, v);
            }

            if(72 <= position && position < 75)
                return Colors.Black;

            if(75 <= position && position < 88)
            {
                var intensity = (position - 75) / 8;
                return rainbow.GetColor(light, 0, ms) * intensity;
            }

            return rainbow.GetColor(light, 0, ms);
        }
    }
}