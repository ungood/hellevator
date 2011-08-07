using System;
using System.Threading;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;
using Hellevator.Behavior.Animations;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    public class LPD6803
    {
        public int NumPixels { get; private set; }

        private readonly SPI spi;
        private readonly ushort[] buffer;
        private readonly OutputCompare oc;
        
        public LPD6803(SPI.SPI_module spiModule, int numPixels)
        {
            var spiConfig = new SPI.Configuration(Cpu.Pin.GPIO_NONE, true, 0, 0, false, true, 500, spiModule);
            spi = new SPI(spiConfig);
            
            NumPixels = numPixels;
            
            // On a LPD6803, the PWM counter is shared with the data pin.
            // This means, when we send data, we must align the data to 256 bits (32 bytes, 16 ushorts)
            // We also have to consider the 4 byte header.
            var bufferLength = numPixels + 2;
            var alignment = 32 - (bufferLength % 32);
            bufferLength += alignment;
            Debug.Assert(bufferLength % 32 == 0);

            buffer = new ushort[bufferLength];

            oc = new OutputCompare((Cpu.Pin) FEZ_Pin.Digital.Di10, false, 2);
            oc.Set(false, new uint[] {1, 1}, 0, 2, true);
        }



        private void WriteLoop(object state)
        {
            spi.Write(buffer);
        }

        public void SetColor(int pixel, byte red, byte green, byte blue)
        {
            Debug.Assert(pixel < NumPixels);

            var r = red >> 3;
            var g = green >> 3;
            var b = blue >> 3;

            // Bit pattern for color data: 1rrrrrgggggbbbbb
            buffer[pixel+2] = (ushort) (0x8000 | r << 10 | g << 5 | b);
        }

        public void Update()
        {
            oc.Set(false);
            spi.Write(buffer);
            oc.Set(false, new uint[] {100, 100}, 0, 2, true);
        }
    }
}
