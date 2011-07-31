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
        private static readonly RainbowEffect Rainbow = new RainbowEffect(1, 10);

        public override Color GetColor(double light, double floor, long ms)
        {
            var position = floor + 0.5 + light;

            if(position <= 24)
                return Colors.Black;

            if(position >= 72)
                return Rainbow.GetColor(light, 0, ms);

            // white -> blue -> black;
            if(24 < position && position <= 36)
            {
                var s = (position - 24) / 12;
                var v = 1 - s;
                return Color.FromHSV(240, s, v);
            }

            // Black with white "stars"
            if(36 < position && position <= 60)
            {
                var f = (int) (position * 15);
                if(f % 7 == 0 && f % 2 != 0)
                {
                    return Colors.White;
                }
                
                return Colors.Black;
            }
            
            // else 60 < position && position < 72
            var intensity = (position - 60) / 12;
            return Rainbow.GetColor(light, 0, ms) * intensity;
        }
    }
}