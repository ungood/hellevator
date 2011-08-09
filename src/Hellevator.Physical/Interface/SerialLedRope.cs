using System;
using System.IO.Ports;
using Hellevator.Behavior.Effects;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;

namespace Hellevator.Physical.Interface
{
    public class SerialLedRope : ILightStrip
    {
        private readonly byte address;
        public int NumLights { get; private set; }
        
        private readonly SerialPort port;
        private readonly byte[] buffer;

        public SerialLedRope(string comPort, char address, int numLights)
            : this(comPort, (byte)address, numLights) {}

        public SerialLedRope(string comPort, byte address, int numLights)
        {
            this.address = address;
            port = new SerialPort(comPort, 115200);
            port.Open();
        
            NumLights = numLights;
            buffer = new byte[2 + (3 * numLights)];
            Reset();
        }

        public void SetColor(int light, Color color)
        {
            if(light < 0 || light >= NumLights)
                return;

            buffer[3 * light + 1] = (byte) (color.Red / 2);
            buffer[3 * light + 2] = (byte) (color.Green / 2);
            buffer[3 * light + 3] = (byte) (color.Blue / 2);
        }

        public void Update()
        {
            port.Write(buffer, 0, buffer.Length);
        }

        public void Reset()
        {
            buffer[0] = (byte) (0x80 | address);
            buffer[buffer.Length - 1] = 0x80;
            for(int i = 1; i < buffer.Length - 1; i++)
                buffer[i] = 0;
            Update();
        }
    }
}
