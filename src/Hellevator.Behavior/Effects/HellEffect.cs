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
    public class HellEffect : Effect
    {
        public override Color GetColor(double light, double floor, long ms)
        {
            var sec = ms / 125.0;

            //var w1 = Pulse(sec, 10) / 3; // 0.2 - 0.7
            var w2 = Pulse(sec, 3)  / 2;
            var w3 = Pulse(sec, 1)  / 2;
            var red = w2 + w3;
            
            var green = Pulse(sec + light, 2) * (red / 2.5);
            return new Color(red, green, 0);
        }

        private static double Pulse(double value, double period)
        {
            value /= period;
            value = 2 * (value - Math.Floor(value + 0.5));
            return value < 0 ? value * -1 : value;
        }
    }
}