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

namespace Hellevator.Behavior.Animations
{
    public class BounceEase : EasingFunction
    {
        private const double P1 = 0.2;
        private const double P2 = 1.8;
        private const double P3 = 28.795;
        private const double P4 = -65.89;
        private const double P5 = 36.095;

        protected override double EaseIn(double time)
        {
            var t2 = time * time;
            var t3 = time * time * time;
            return (P5 * t2 * t3) + (P4 * t2 * t2) + (P3 * t3) + (P2 * t2) + (P1 * time);
        }
    }
}