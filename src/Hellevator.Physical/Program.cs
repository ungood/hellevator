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
using GHIElectronics.NETMF.FEZ.Shields;
using GHIElectronics.NETMF.IO;
using Hellevator.Physical.Components;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical
{
    public class Program
    {
        private static readonly PersistentStorage sd = new PersistentStorage("SD");
        private static readonly SpiCoordinator coord = new SpiCoordinator(SPI.SPI_module.SPI1);

        private static readonly AudioShieldPlayer player = new AudioShieldPlayer(coord,
            (Cpu.Pin) FEZ_Pin.Digital.An4,
            (Cpu.Pin) FEZ_Pin.Digital.An5,
            (Cpu.Pin) FEZ_Pin.Digital.Di4);

        public static void Main()
        {
            Debug.Print("Starting");

            sd.MountFileSystem();

            player.Initialize();
            Debug.Print("Initialized");
            using(var stream = new FileStream(@"\SD\sample.ogg", FileMode.Open, FileAccess.Read, FileShare.Read, 32))
            {
                //    music.Play(stream);
                //    while(music.IsBusy)
                //        Thread.Sleep(10);
                player.Play(stream);
                while(player.IsPlaying)
                    Thread.Sleep(10);
            }

            Debug.Print("Done");
        }

        private static void PlaybackTest()
        {
            
        }

        private static void Testing(Widget w)
        {
            Debug.Print(w.Name);
            Debug.GC(true);
        }

        private static void ShiftRegisterTest(double value)
        {
            var module = SPI.SPI_module.SPI2;
            var latchPin = (Cpu.Pin) FEZ_Pin.Digital.Di52;
            var spiConfig = new SPI.Configuration(latchPin, true, 0, 0, false, true, 20000, module);
            var spi = new SPI(spiConfig);

            
            
            //shift.ShiftOut(0, 0, 0);
            //Thread.Sleep(1000);
            //shift.ShiftOut(16, 0, 0);

            //var led = new OutputPort((Cpu.Pin) FEZ_Pin.Digital.LED, true);

            var on = new byte[] {0, 1, 0};
            var off = new byte[] {0, 0, 0};
            var start = Utility.GetMachineTime();
            for(var i = 0; i < 10000; i++)
            {
                for(byte intensity = 0; intensity < 255; intensity++)
                {
                    if(intensity < 25)
                        spi.Write(on);
                    else
                        spi.Write(off);
                }
            }
            var end = Utility.GetMachineTime();

            Debug.Print((end - start).Milliseconds.ToString());
        }


    }

    struct Widget
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
