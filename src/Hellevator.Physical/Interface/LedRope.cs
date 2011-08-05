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

using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Effects;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Components;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Interface
{
    public class LedRope : ILightStrip
    {
        public int NumLights { get; private set; }
        
        private readonly LPD6803 shift;
        private readonly Color[] backBuffer;

        public LedRope(SPI.SPI_module spi, int numLights)
        {
            shift = new LPD6803(spi, numLights);
            NumLights = numLights;
            backBuffer = new Color[numLights];
        }

        public void SetColor(int light, Color color)
        {
            shift.Set(light, color.Red, color.Green, color.Blue);
        }

        public void Update()
        {
            shift.Update();
        }
    }
}
