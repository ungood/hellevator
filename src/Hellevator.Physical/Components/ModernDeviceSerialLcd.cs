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
            port.Write("?G420?BFF");  // Set Geometry to 4x20 and backlight full on
            Thread.Sleep(100);
        }

        public void Display(int line, string message)
        {
            port.Write("?x00?y" + line + "?l" + message);
        }
    }
}
