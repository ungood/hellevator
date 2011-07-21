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
    /// <summary>
    /// An effect that mimics the lights you might see between the doors while riding an elevator.
    /// </summary>
    public class ElevatorEffect : Effect
    {
        public static readonly ElevatorEffect Instance = new ElevatorEffect();

        /// <summary>
        /// The height of a door, as a ratio to the make believe distance between floors.
        /// </summary>
        private const double DoorHeight = 0.5;

        private const double HalfDoorHeight = DoorHeight / 2;

        private const double BlurDistance = 0.2;

        public override Color GetColor(double light, double floor, long ticks)
        {
            var position = floor + light + 0.5;
            var x = position - (int) position;

            var floorIndex = (int)Math.Round(position);
            var floorColor = floorIndex < 0 || floorIndex > 23 ? Colors.Red : FloorColors[floorIndex];

            if(HalfDoorHeight > x || x > 1 - HalfDoorHeight)
                return floorColor;

            var distance = x > 0.5 ? 1 - HalfDoorHeight - x : x - HalfDoorHeight;
            if(distance > BlurDistance)
                return Colors.Black;

            var intensity = 1 - (distance / BlurDistance);
            return floorColor * intensity;
        }

        static ElevatorEffect()
        {
            FloorColors = new Color[24];
            for(int i = 0; i < 24; i++)
            {
                FloorColors[i] = Color.FromHSV(10 * (23-i), 1.0, 1.0);
            }
        }

        private static readonly Color[] FloorColors;
    }
}