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
    public class FloorEffect : Effect
    {
        /// <summary>
        /// The height of a door, as a ratio to the make believe distance between floors.
        /// </summary>
        public double DoorHeight { get; private set; }

        public double HalfDoorHeight { get; private set; }
        
        public double BlurDistance { get; private set; }

        private readonly HellEffect hell = new HellEffect();

        public FloorEffect(double doorHeight = 0.35, double blurDistance = 0.1)
        {
            DoorHeight = doorHeight;
            HalfDoorHeight = doorHeight / 2;
            BlurDistance = blurDistance;
        }

        public override Color GetColor(double light, double floor, long ms)
        {
            var position = CalcPosition(light, floor);
            var x = position - (int) position;
            x = x < 0 ? x * -1 : x;

            var floorIndex = (int) Math.Round(position) - 1;
            if(floorIndex > 23)
                return Colors.Black;
            
            var floorColor = floorIndex < 0 ? hell.GetColor(light, floor, ms) : FloorColors[floorIndex];

            if(HalfDoorHeight > x || x > 1 - HalfDoorHeight)
                return floorColor;

            var distance = x > 0.5 ? 1 - HalfDoorHeight - x : x - HalfDoorHeight;
            if(distance > BlurDistance)
                return Colors.Black;

            var intensity = 1 - (distance / BlurDistance);
            return floorColor * intensity;
        }

        static FloorEffect()
        {

            // GOLD COLOR is HSV 60, 1.0, 1.0
            FloorColors = new Color[24];
            for(int i = 0; i < 24; i++)
            {
                FloorColors[i] = Color.FromHSV(60 + i * 8, 1.0, 1.0);
            }
        }

        private static readonly Color[] FloorColors; 
    }
}