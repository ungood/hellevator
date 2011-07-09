using System;
using System.Threading;
using Hellevator.Behavior.Animations;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    public class LPD6803
    {
        public int NumPixels { get; private set; }

        private readonly SPI spi;
        private readonly ushort[][] buffers;
        private int frontBuffer;
        private int backBuffer = 1;
        
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

            buffers = new [] {
                new ushort[bufferLength],
                new ushort[bufferLength]
            };
            
            new Thread(WriteLoop).Start();
        }

        private void WriteLoop()
        {
            while(true)
            {
                spi.Write(buffers[frontBuffer]);
            }
        }

        public void Set(int pixel, byte red, byte green, byte blue)
        {
            Debug.Assert(pixel < NumPixels);

            var r = red >> 3;
            var g = green >> 3;
            var b = blue >> 3;

            // Bit pattern for color data: 1rrrrrgggggbbbbb
            buffers[backBuffer][pixel] = (ushort) (0x8000 | r << 10 | g << 5 | b);
        }
        
        /// <summary>
        /// Update the led pixels by swapping the front and back buffers.
        /// </summary>
        public void Update()
        {
            backBuffer = Interlocked.Exchange(ref frontBuffer, backBuffer);
        }
    }
}
