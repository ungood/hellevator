using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    /// <summary>
    /// Responsible for coordinating access to a single SPI port across multiple threads
    /// </summary>
    public class SpiCoordinator
    {
        private const long TickInterval = TimeSpan.TicksPerMillisecond * 10;

        private readonly SpiWriter[] writers;
        private readonly SPI spi;
        
        public SPI.SPI_module Module { get; private set; }
        public bool IsInitialized { get; private set; }

        private readonly Thread loopThread;
        
        public SpiCoordinator(SPI.SPI_module module, int maxSize)
        {
            Module = module;
            var blankConfig = new SPI.Configuration(Cpu.Pin.GPIO_NONE, false, 0, 0, false, true, 2000, module);
            spi = new SPI(blankConfig);

            writers = new SpiWriter[maxSize];

            loopThread = new Thread(Loop);
        }

        private int writerIndex;
        public void Add(SpiWriter writer)
        {
            if(IsInitialized)
                throw new InvalidOperationException(
                    "Cannot add new SpiWriters after the the SpiCoordinator has been Initialized");
            writers[writerIndex++] = writer;
        }

        public void Initialize()
        {
            loopThread.Start();
            IsInitialized = true;
        }

        public void Loop()
        {
            while(true)
            {
                for(int i = 0; i < writers.Length; i++)
                {
                    writers[i].WriteInternal(spi);
                }
                //Thread.Sleep(1);
            }
        }
    }
}