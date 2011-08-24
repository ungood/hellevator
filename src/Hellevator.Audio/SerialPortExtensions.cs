using System;
using System.IO.Ports;
using System.Text;
using Microsoft.SPOT;

namespace Hellevator.Audio
{
    public static class SerialPortExtensions
    {
        public static void Write(this SerialPort port, string value)
        {
            var buffer = Encoding.UTF8.GetBytes(value);
            port.Write(buffer, 0, buffer.Length);
        }

        public static byte ReadByte(this SerialPort port)
        {
            var received = new byte[1];
            port.Read(received, 0, 1);
            return received[0];
        }

        public static int Write(this SerialPort port, params byte[] buffer)
        {
            return port.Write(buffer, 0, buffer.Length);
        }
    }
}
