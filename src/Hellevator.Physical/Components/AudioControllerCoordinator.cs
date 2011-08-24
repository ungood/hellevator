using System;
using System.IO.Ports;
using System.Text;
using Microsoft.SPOT;

namespace Hellevator.Physical.Components
{
    public enum AudioControllerCommand
    {
        Play = 0,
        Loop,
        Stop,
        Fade
    }

    public class AudioControllerCoordinator
    {
        private readonly SerialPort port;
        private readonly object portLock = new object();

        public AudioControllerCoordinator(string portName)
        {
            port = new SerialPort(portName, 115200);
            port.Open();
        }

        public void SendCommand(byte address, AudioControllerCommand command, string data = null)
        {
            if(data == null)
                data = "";

            var buffer = new byte[3 + data.Length];

            buffer[0] = (byte) (0x80 | address);
            buffer[1] = (byte) command;
            buffer[buffer.Length - 1] = 0;

            var bytes = Encoding.UTF8.GetBytes(data);
            Array.Copy(bytes, 0, buffer, 2, bytes.Length);

            lock(portLock)
            {
                port.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
