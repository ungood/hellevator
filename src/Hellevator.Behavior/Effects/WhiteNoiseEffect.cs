﻿#region License
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
    public class WhiteNoiseEffect : Effect
    {
        private double[] prev = new double[72];
        
        public override Color GetColor(double light, double floor, long ms)
        {
            var intensity = GetIntensity(light);
            return new Color(intensity, intensity, intensity);
        }

        public double GetIntensity(double light)
        {
            var rnd = (double) (RNG.Next() % 256) / 256;
            var index = (int) (70 * light);
            var intensity = (prev[index] + rnd) / 2;

            prev[index] = intensity;
            return intensity;
        }
    }
}