using System;
using Hellevator.Behavior.Animations;
using Hellevator.Behavior.Interface;
using Hellevator.Physical.Components;
using Microsoft.SPOT;
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
            shift = new LPD6803(spi, 68);
            NumLights = numLights;
            backBuffer = new Color[numLights];
        }

        public void SetColor(int light, Color color)
        {
            backBuffer[light] = color;
        }

        public void Update()
        {
            shift.WriteColors(backBuffer);
        }
    }
}
