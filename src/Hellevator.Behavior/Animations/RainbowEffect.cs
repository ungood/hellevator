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

using System;

namespace Hellevator.Behavior.Animations
{
    public class RainbowEffect : Effect
    {
        public override Color GetColor(int index, double floor, long ticks)
        {
            var floorProgress = floor;
            var progress = (double) index / 30;
            var hue = (floorProgress + progress);
            hue -= Math.Floor(hue);

            return Color.FromHSL(hue, 0.5, 0.5);
        }

        private const double Fact3 = 3 * 2;
        private const double Fact5 = 5 * 4 * Fact3;
        private const double Fact7 = 7 * 6 * Fact5;

        private static double Greyscale(double input)
        {
            return (Sin(input) + 1) / 2;
        }

        private static double Sin(double rad)
        {
            return rad - (Math.Pow(rad, 3) / Fact3) + (Math.Pow(rad, 5) / Fact5) - (Math.Pow(rad, 7) / Fact7);
        }
    }
}