using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using GHIElectronics.NETMF.IO;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;

namespace Hellevator.Audio
{
    public class Program
    {
        private static readonly SerialReceiver receiver = new SerialReceiver("COM1", 115200, 1);
        private static readonly AudioShield shield = new AudioShield(SPI.SPI_module.SPI1,
                (Cpu.Pin)FEZ_Pin.Digital.An4,
                (Cpu.Pin)FEZ_Pin.Digital.An5,
                (Cpu.Pin)FEZ_Pin.Digital.Di4);
        
        public static void Main()
        {
            var storage = new PersistentStorage("SD");
            storage.MountFileSystem();

            shield.Initialize();

            receiver.CommandReceived += CommandReceived;

            // TEST
            CommandReceived(null, new ControllerEventArgs {
                Command = CommandType.Play,
                Data = (byte) Playlist.ElevatorMusic.Index
            });

            Thread.Sleep(3000);

            CommandReceived(null, new ControllerEventArgs {
                Command = CommandType.Stop
            });

            Thread.Sleep(Timeout.Infinite);
        }

        private static void CommandReceived(object sender, ControllerEventArgs e)
        {
            switch(e.Command)
            {
                case CommandType.Play:
                    var filename = @"\SD\" + Playlist.GetPlaylist(e.Data).GetNext() + ".ogg";
                    var stream = new FileStream(filename, FileMode.Open);
                    shield.Play(stream);
                    break;

                case CommandType.Loop:
                case CommandType.Fade:
                    break;

                case CommandType.Stop:
                    shield.Stop();
                    break;
            }
        }
    }
}
