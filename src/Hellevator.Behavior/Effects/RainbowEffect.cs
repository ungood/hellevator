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
    /// <summary>
    /// A slowly changing pastel rainbow.
    /// </summary>
    public class RainbowEffect : Effect
    {
        private readonly double lightMult;
        private readonly double timeMult;

        /// <summary>
        /// Create a rainbow effect with a given wavelength and period.
        /// </summary>
        /// <param name="wavelength"></param>
        /// <param name="period"></param>
        public RainbowEffect(double wavelength, double period)
        {
            lightMult = 360 / wavelength;
            timeMult = 360 / period;
        }

        public override Color GetColor(double light, double floor, long ms)
        {
            var sec = (double)ms / 1000;
            var hue = (light * lightMult) + (sec * timeMult);
            
            return Color.FromHSV(hue, 0.5, 1.0);
        }
    }
}