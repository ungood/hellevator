using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    /// <summary>
    /// Responsible for coordinating access to a single SPI port across multiple threads
    /// </summary>
    public class SpiCoordinator
    {
        private readonly SPI spi;
        private readonly object spiLock = new object();

        public delegate void SpiAction(SPI spi);

        public SpiCoordinator(SPI.SPI_module module)
        {
            var blankConfig = new SPI.Configuration(Cpu.Pin.GPIO_NONE, false, 0, 0, false, true, 2000, module);
            spi = new SPI(blankConfig);
        }

        public SPI.SPI_module Module
        {
            get { return spi.Config.SPI_mod; }
        }

        public void Execute(SPI.Configuration config, SpiAction action)
        {
            lock(spiLock)
            {
                spi.Config = config;
                action(spi);
            }
        }
    }
}