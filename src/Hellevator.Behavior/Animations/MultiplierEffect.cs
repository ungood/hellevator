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

namespace Hellevator.Behavior.Animations
{
    public class MultiplierEffect : Effect
    {
        private readonly Effect left;
        private readonly Effect right;

        public MultiplierEffect(Effect left, Effect right)
        {
            this.left = left;
            this.right = right;
        }

        public override Color GetColor(int index, int numLights, double floor, long ticks)
        {
            return left.GetColor(index, numLights, floor, ticks) * right.GetColor(index, numLights, floor, ticks);
        }
    }
}