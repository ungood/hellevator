using System;
using System.IO.Ports;
using System.Text;
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
        public string Data { get; set; }
    }

    public delegate void ControllerEventHandler(object sender, ControllerEventArgs e);

    public class SerialReceiver
    {
        private const int BufferSize = 100;
        private readonly byte[] buffer = new byte[BufferSize];
        private int index;
        private CommandType command;
        
        private readonly SerialPort port;
        private readonly byte myAddress;
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
            var data = port.ReadByte();

            if((data & 0x80) != 0)
            {
                index = -1;
                var address = data & 0x7f;
                if(address == 0 || address == myAddress)
                {
                    port.Write("RECEIVING");
                    receiving = true;
                }
                else
                {
                    port.Write("IGNORING");
                    receiving = false;
                }

                return;
            }
            
            if(receiving)
            {
                if(index >= BufferSize)
                {
                    port.Write("BUFFER OVERFLOW!");
                    receiving = false;
                    return;
                }

                if(index < 0)
                {
                    command = (CommandType) (data);
                    port.Write("COMMAND RECEIVED: " + command);
                    index = 0;
                    return;

                }

                if(data == 0x00)
                {
                    var temp = new byte[index];
                    Array.Copy(buffer, temp, index);
                    var s = new string(Encoding.UTF8.GetChars(temp));
                    var args = new ControllerEventArgs {
                        Command = command,
                        Data = s
                    };
                    OnCommandReceived(args);
                    port.Write("DATA RECEIVED: " + s);
                    receiving = false;
                    return;
                }

                buffer[index++] = data;
            }

        }
        
        protected void OnCommandReceived(ControllerEventArgs e)
        {
            var data = e.Data ?? "NULL";
            Debug.Print("Command Received: " + e.Command.ToString() + " " + data);

            ControllerEventHandler handler = CommandReceived;
            if(handler != null)
                handler(this, e);
        }
    }
}
