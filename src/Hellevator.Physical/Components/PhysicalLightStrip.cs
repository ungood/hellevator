using System;
using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;

namespace Hellevator.Physical.Components
{
    public class PhysicalLightStrip : ILightStrip
    {
        public int NumLights
        {
            get { return 24; }
        }

        public void SetColor(int light, Color color)
        {
            
        }

        public void Update()
        {
            
        }
    }
}
