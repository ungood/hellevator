using System;
using System.IO.Ports;
using System.Text;
using Microsoft.SPOT;

namespace Hellevator.Physical.Components
{
    public static class SerialPortExtensions
    {
        public static void Write(this SerialPort port, string value)
        {
            var buffer = Encoding.UTF8.GetBytes(value);
            port.Write(buffer, 0, buffer.Length);
        }
    }
}
