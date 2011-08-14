using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    public class BufferSpiWriter : SpiWriter
    {
        public BufferSpiWriter(SPI.Configuration configuration, InputPort dreq)
            : base(configuration, dreq) {}
        private byte[] buffer;

        public void Write(params byte[] data)
        {
            BusyEvent.WaitOne();

            BusyEvent.Reset();
            buffer = data;

            BusyEvent.WaitOne();
        }

        protected override byte[] GetData()
        {
            if(buffer == null || DataRequest.Read() == false)
                return null;

            var temp = buffer;
            buffer = null;
            BusyEvent.Set();
            return temp;
        }
    }
}