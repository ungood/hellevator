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
    /// <summary>
    /// An effect that mimics the lights you might see between the doors while riding an elevator.
    /// </summary>
    public class ElevatorEffect : Effect
    {
        private readonly HeavenEffect heaven = new HeavenEffect();
        private readonly FloorEffect floors = new FloorEffect();

        public override Color GetColor(double light, double floor, long ms)
        {
            var position = CalcPosition(light, floor);

            if(position > 25)
                return heaven.GetColor(light, floor, ms);
            
            return floors.GetColor(light, floor, ms);
        }
    }
}