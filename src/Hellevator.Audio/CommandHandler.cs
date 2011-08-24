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

using System;
using System.IO;
using System.Threading;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Audio
{
    public class CommandHandler
    {
        private readonly AudioShieldPlayer player = new AudioShieldPlayer(SPI.SPI_module.SPI1,
            (Cpu.Pin)FEZ_Pin.Digital.An4,
            (Cpu.Pin)FEZ_Pin.Digital.An5,
            (Cpu.Pin)FEZ_Pin.Digital.Di4);

        private readonly OutputPort isPlaying;

        private string currentFilename;
        private bool isLooping;

        private readonly Random random;

        public CommandHandler(FEZ_Pin.Digital isPlayingPin)
        {
            isPlaying = new OutputPort((Cpu.Pin) isPlayingPin, false);
            
            player.Initialize();
            player.PlayFinished += PlayFinished;

            var seed = new AnalogIn((AnalogIn.Pin) FEZ_Pin.AnalogIn.An3);
            seed.SetLinearScale(0, 3300);
            random = new Random(seed.Read());
        }

        public void HandleCommand(object sender, ControllerEventArgs e)
        {
            switch(e.Command)
            {
                case CommandType.Play:
                    Play(e.Data);
                    break;
                case CommandType.Loop:
                    Loop(e.Data);
                    break;

                case CommandType.Stop:
                    Stop();
                    break;

                case CommandType.Fade:
                    FadeOut();
                    break;
            }
        }

        private void Start(string filename)
        {
            currentFilename = filename;
            
            if(Directory.Exists(filename))
            {
                var files = Directory.GetFiles(filename);
                filename = files[random.Next(files.Length)];
            }

            if(!File.Exists(filename))
                return;

            if(player.IsPlaying)
            {
                player.Stop();
                while(player.IsPlaying)
                    Thread.Sleep(10);
            }

            var stream = new FileStream(filename, FileMode.Open);
            isPlaying.Write(true);
            player.Play(stream);
        }

        private void PlayFinished()
        {
            if(isLooping)
            {
                Start(currentFilename);
                return;
            }

            isPlaying.Write(false);
        }

        private void Play(string data)
        {
            Stop();
            isLooping = false;
            Start(@"\SD\" + data);
        }

        private void Loop(string data)
        {
            Stop();
            isLooping = true;
            Start(@"\SD\" + data);
        }

        private void Stop()
        {
            isLooping = false;

            if(player.IsPlaying)
                player.Stop();
            while(player.IsPlaying)
                Thread.Sleep(1);

            isPlaying.Write(false);
        }

        private void FadeOut()
        {
            isLooping = false;

            for(var volume = 255; volume >= 0; volume -= 5)
            {
                player.SetVolume((byte) volume, (byte) volume);
                Thread.Sleep(1);
            }

            Stop();
        }
    }
}