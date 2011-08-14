using System.IO;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    public class StreamSpiWriter : SpiWriter
    {
        private Stream readStream;
        private readonly byte[] buffer = new byte[BufferSize];

        public StreamSpiWriter(SPI.Configuration configuration, InputPort dreq)
            : base(configuration, dreq) {}

        public void Play(Stream stream)
        {
            // TODO: What if a song is already playing?

            BusyEvent.Reset();
            readStream = stream;
        }

        protected override byte[] GetData()
        {
            if(readStream == null || DataRequest.Read() == false)
                return null;

            var bytesRead = readStream.Read(buffer, 0, BufferSize);
            if(bytesRead < 1)
            {
                readStream.Close();
                readStream = null;
                BusyEvent.Set();
                //TODO: Reset
                return null;
            }

            return buffer;
        }
    }
}