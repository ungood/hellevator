using System;
using System.IO.Ports;
using Microsoft.SPOT;

namespace Hellevator.Audio
{
    public enum CommandType
    {
        Play = 0x00,
        Loop = 0x01,
        Stop = 0x02,
        Fade = 0x03
    }

    public class ControllerEventArgs : EventArgs
    {
        public CommandType Command { get; set; }
        public byte Data { get; set; }
    }

    public delegate void ControllerEventHandler(object sender, ControllerEventArgs e);

    public class SerialReceiver
    {
        private readonly SerialPort port;
        private readonly byte myAddress;

        private readonly byte[] buffer = new byte[0];

        private bool receiving;

        public event ControllerEventHandler CommandReceived;

        public SerialReceiver(string portName, int baud, byte address)
        {
            myAddress = address;
            port = new SerialPort(portName, baud);
            port.DataReceived += DataReceived;
            port.Open();
        }


        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            port.Read(buffer, 0, 1);

            var data = buffer[0];

            if((data & 0x80) == 1)
            {
                var address = data & 0x7f;
                if(address == 0 || address == myAddress)
                    receiving = true;
            }
            else if(receiving)
            {
                OnCommandReceived(new ControllerEventArgs {
                    Command = (CommandType) ((data & 0x60) >> 5),
                    Data = (byte)(data & 0x1f)
                });
            }
        }

        protected void OnCommandReceived(ControllerEventArgs e)
        {
            Debug.Print("Command Received: " + e.Command.ToString() + " " + e.Data.ToString());

            ControllerEventHandler handler = CommandReceived;
            if(handler != null)
                handler(this, e);
        }
    }
}
