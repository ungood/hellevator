using System;
using System.IO.Ports;
using System.Threading;
using Microsoft.SPOT;

namespace Hellevator.Physical.Components
{
    public class ModernDeviceSerialLcd
    {
        private readonly SerialPort port;

        public ModernDeviceSerialLcd(string comPort)
        {
            port = new SerialPort(comPort, 9600);
            port.Open();
        }

        public void Initialize()
        {
            Thread.Sleep(1000);
            port.Write("?G420?BFF");  // Set Geometry to 4x20 and backlight full on
            Thread.Sleep(2000);
            Display(0, "HELLEVATOR 2011");
        }

        public void Display(int line, string message)
        {
            var spacing = (20 - message.Length) / 2;
            port.Write("?y" + line + "?l?x" + Pad(spacing) + message);
        }

        public string Pad(int value)
        {
            return value < 10 ? "0" + value : value.ToString();
        }
    }
}
