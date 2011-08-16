#region License
// Copyright 2011 Jason Walker
// ungood@onetrue.name
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

using System.IO;
using System.Threading;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.IO;
using Hellevator.Behavior.Interface;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Audio
{
    public class Program
    {
        private static readonly SerialReceiver Receiver = new SerialReceiver("COM1", 115200, 1);
        private static readonly AudioShieldPlayer Player = new AudioShieldPlayer(SPI.SPI_module.SPI1,
                (Cpu.Pin)FEZ_Pin.Digital.An4,
                (Cpu.Pin)FEZ_Pin.Digital.An5,
                (Cpu.Pin)FEZ_Pin.Digital.Di4);
        
        public static void Main()
        {
            var storage = new PersistentStorage("SD");
            storage.MountFileSystem();

            Player.Initialize();

            Receiver.CommandReceived += CommandReceived;
            Player.PlayFinished += PlayFinished;

            while(true)
                Thread.Sleep(1000);
        }

        static void PlayFinished()
        {
            Receiver.Send((byte) '!');
        }

        private static Thread commandThread;
        private static bool stopRequested;
        
        private static void RequestStop()
        {
            stopRequested = true;
        }

        private static void CommandReceived(object sender, ControllerEventArgs e)
        {
            switch(e.Command)
            {
                case CommandType.Play:
                    if(Player.IsPlaying)
                        Player.Stop();

                    RequestStop();

                    var filename = @"\SD\" + Playlist.GetPlaylist(e.Data).GetNext() + ".ogg";
                    var stream = new FileStream(filename, FileMode.Open);
                    Player.Play(stream);
                    break;

                case CommandType.Loop:
                    // TODO?
                    //if(Player.IsPlaying)
                    //    Player.Stop();

                    //RequestStop();

                    //playlist = Playlist.GetPlaylist(e.Data);
                    //commandThread = new Thread(Loop);
                    //commandThread.Start();
                    break;

                case CommandType.Stop:
                    RequestStop();
                    Player.Stop();
                    break;

                case CommandType.Fade:
                    RequestStop();
                    commandThread = new Thread(FadeOut);
                    commandThread.Start();
                    break;
            }
        }

        private static void FadeOut()
        {
            stopRequested = false;

            for(int volume = 255; volume >= 0; volume -= 5)
            {
                if(stopRequested)
                {
                    Player.SetVolume(255, 255);
                    return;
                }

                Player.SetVolume((byte)volume, (byte)volume);
                Thread.Sleep(1);
            }

            Player.Stop();
        }
    }
}
