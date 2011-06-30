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

namespace Hellevator.Behavior.Animations
{
    public class ElevatorEffect : Effect
    {
        private EasingFunction intensityEase = new ExponentialEase(5) {Mode = EasingMode.In};

        public override Color GetColor(int index, int numLights, double floor, long ticks)
        {
            // |-A-----B-|
            // 012345678910
            // 10-2+8 = 14 % 10
            // 10-8+2 = 4  % 10

            var x = floor - (int) floor;
            var y = (double) index / numLights;
            var distance = x > y ? x - y : y - x; // make it always positive.
            
            distance = distance > 0.5 ? 1-distance : distance;

            var intensity = 1 - (distance * 2);
            intensity = intensityEase.Ease(intensity);
            
            return new Color(intensity, intensity, intensity);
        }
    }
}