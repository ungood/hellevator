#region License
// Copyright 2011 GHI Electronics LLC
// Modified  2011 Jason Walker
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
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    public class AudioShield
    {
        #region Constants

        private const ushort ClockFreq = 0xa000;

        private enum Direction : byte
        {
            Write = 0x02,
            Read  = 0x03,
        }

        [Flags]
        private enum Mode : ushort
        {
            Reset  = 0x0004,
            Cancel = 0x0010,
            Tests  = 0x0020,
            SdiNew = 0x0800,
            ADPCM  = 0x1000,
            Line1  = 0x4000,
        }

        private enum Register : byte
        {
            /// <summary>
            /// R/W Mode control
            /// </summary>
            Mode      = 0x00,

            /// <summary>
            /// R/W Status of VS1053b
            /// </summary>
            Status    = 0x01,
            
            /// <summary>
            /// R/W Bass/Treble control
            /// </summary>
            Bass      = 0x02,

            /// <summary>
            /// R/W Clock Frequency + Multiplier
            /// </summary>
            ClockFreq = 0x03,

            /// <summary>
            /// R/W Volume control
            /// </summary>
            Volume    = 0x0B,
            
        }

        #endregion

        #region Old Stuff

        ///// <summary>
        ///// Volume control
        ///// R/W
        ///// </summary>
        //private const int SCI_WRAM = 0x06;

        ///// <summary>
        ///// Volume control
        ///// R/W
        ///// </summary>
        //private const int SCI_WRAMADDR = 0x07;

        ///// <summary>
        ///// Stream header data 0
        ///// R
        ///// </summary>
        //private const int SCI_HDAT0 = 0x08;

        ///// <summary>
        ///// Stream header data 1
        ///// R
        ///// </summary>
        //private const int SCI_HDAT1 = 0x09;

        ///// <summary>
        ///// Volume control
        ///// R/W
        ///// </summary>
        //private const int SCI_AIADDR = 0x0A;

        ///// <summary>
        ///// Application control register 0
        ///// R/W
        ///// </summary>
        //private const int SCI_AICTRL0 = 0x0C;

        ///// <summary>
        ///// Application control register 1
        ///// R/W
        ///// </summary>
        //private const int SCI_AICTRL1 = 0x0D;

        ///// <summary>
        ///// Application control register 2
        ///// R/W
        ///// </summary>
        //private const int SCI_AICTRL2 = 0x0E;

        ///// <summary>
        ///// Application control register 3
        ///// R/W
        ///// </summary>
        //private const int SCI_AICTRL3 = 0x0F;

        #endregion

        #region Recording constants

        private const int PCM_MODE_JOINTSTEREO = 0x00;
        private const int PCM_MODE_DUALCHANNEL = 0x01;
        private const int PCM_MODE_LEFTCHANNEL = 0x02;
        private const int PCM_MODE_RIGHTCHANNEL = 0x03;

        private const int PCM_ENC_ADPCM = 0x00;
        private const int PCM_ENC_PCM = 0x04;

        #endregion

        #region Hardware Resources

        private readonly SpiCoordinator coordinator;
        private readonly SPI.Configuration dataConfig;
        private readonly SPI.Configuration cmdConfig;
        private readonly InterruptPort dreq;

        #endregion

        public AudioShield(SpiCoordinator coordinator, Cpu.Pin dataSelectPin, Cpu.Pin cmdSelectPin, Cpu.Pin dreqPin)
        {
            this.coordinator = coordinator;
            dataConfig = new SPI.Configuration(dataSelectPin, false, 0, 0, false, true, 2000, coordinator.Module, dreqPin, false);
            cmdConfig = new SPI.Configuration(cmdSelectPin, false, 0, 0, false, true, 2000, coordinator.Module, dreqPin, false);
            dreq = new InterruptPort(dreqPin, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeHigh);

            Reset();

            WriteMode(Mode.SdiNew);
            WriteRegister(Register.ClockFreq, ClockFreq);
            
            // Test if initialized
            WriteRegister(Register.Volume, 0x0101);
            if(ReadRegister(Register.Volume) != (0x0101))
            {
                throw new Exception("Failed to initialize MP3 Decoder.");
            }
        }

        #region Commands

        private readonly byte[] cmdBuffer = new byte[4];

        /// <summary>
        /// Reads 16bit value from a register
        /// </summary>
        private ushort ReadRegister(Register register)
        {
            WaitForDataRequest();

            cmdBuffer[0] = (byte) Direction.Read;
            cmdBuffer[1] = (byte) register;
            cmdBuffer[2] = 0;
            cmdBuffer[3] = 0;
            coordinator.Execute(cmdConfig, spi => spi.WriteRead(cmdBuffer, cmdBuffer, 2));

            return (ushort) (cmdBuffer[0] << 8 | cmdBuffer[1]);
        }

        /// <summary>
        /// Writes 16bit value to a register
        /// </summary>
        private void WriteRegister(Register register, ushort data)
        {
            WaitForDataRequest();

            cmdBuffer[0] = (byte) Direction.Write;
            cmdBuffer[1] = (byte) register;
            cmdBuffer[2] = (byte) (data >> 8);
            cmdBuffer[3] = (byte) data;
            coordinator.Execute(cmdConfig, spi => spi.Write(cmdBuffer));
        }

        private void WriteMode(Mode mode)
        {
            WriteRegister(Register.Mode, (ushort) mode);
        }

        private void WaitForDataRequest()
        {
            while(dreq.Read() == false)
                Thread.Sleep(0);
        }

        /// <summary>
        /// Performs soft reset
        /// </summary>
        private void Reset()
        {
            WriteMode(Mode.SdiNew | Mode.Reset);
            Thread.Sleep(1);

            WriteRegister(Register.ClockFreq, ClockFreq);
            WaitForDataRequest();
        }

        /// <summary>
        /// Set volume for both channels. Valid values 0-255
        /// </summary>
        /// <param name="leftChannelVolume">0 - silence, 255 - loudest</param>
        /// <param name="rightChannelVolume">0 - silence, 255 - loudest</param>
        public void SetVolume(byte leftChannelVolume, byte rightChannelVolume)
        {
            WriteRegister(Register.Volume, (ushort) ((255 - leftChannelVolume) << 8 | (255 - rightChannelVolume)));
        }

        #endregion

        

        /// <summary>
        /// Returns true is shield is playing or recording audio
        /// </summary>
        public bool IsBusy
        {
            get { return _isPlaying || _isRecording; }
        }

        private void WritePatchFromArray(ushort[] patch)
        {
            spi.Config = cmdConfig;
            cmdBuffer[0] = 0x02;

            var count = 0;
            var i = 0;
            ushort value = 0;

            while(i < patch.Length)
            {
                cmdBuffer[1] = (byte) patch[i++];
                count = patch[i++];
                if((count & 0x8000) != 0)
                {
                    count &= 0x7FFF;
                    value = patch[i++];
                    cmdBuffer[2] = (byte) (value >> 8);
                    cmdBuffer[3] = (byte) value;

                    while(count-- > 0)
                    {
                        spi.Write(cmdBuffer);
                        while(dreq.Read() == false)
                            ;
                    }
                }
                else
                {
                    while(count-- > 0)
                    {
                        value = patch[i++];
                        cmdBuffer[2] = (byte) (value >> 8);
                        cmdBuffer[3] = (byte) value;
                        spi.Write(cmdBuffer);
                        while(dreq.Read() == false)
                            ;
                    }
                }
            }
        }

        private void WritePatchFromStream(Stream s)
        {
            spi.Config = cmdConfig;
            cmdBuffer[0] = 0x02;

            byte highByte = 0;
            byte lowByte = 0;
            var count = 0;
            while(true)
            {
                while(dreq.Read() == false)
                    Thread.Sleep(1);

                //register address
                cmdBuffer[1] = (byte) s.ReadByte();
                s.ReadByte();
                lowByte = (byte) s.ReadByte();
                highByte = (byte) s.ReadByte();
                count = (highByte & 0x7F) << 8 | lowByte;
                if((highByte & 0x80) != 0)
                {
                    cmdBuffer[3] = (byte) s.ReadByte();
                    cmdBuffer[2] = (byte) s.ReadByte();
                    while(count-- > 0)
                    {
                        spi.Write(cmdBuffer);
                    }
                }
                else
                {
                    while(count-- > 0)
                    {
                        cmdBuffer[3] = (byte) s.ReadByte();
                        cmdBuffer[2] = (byte) s.ReadByte();
                        spi.Write(cmdBuffer);
                    }
                }
            }
        }

        #region VS1053b Tests

        /// <summary>
        /// Run sine test
        /// </summary>
        public void SineTest()
        {
            byte[] buf = {0};

            WriteMode(Mode.SdiNew | Mode.Tests | Mode.Reset);

            byte[] start = {0x53, 0xEF, 0x6E, 0x7E, 0x00, 0x00, 0x00, 0x00};
            byte[] end = {0x45, 0x78, 0x69, 0x74, 0x00, 0x00, 0x00, 0x00};



            spi.Config = dataConfig;


            //Write start sequence
            for(var i = 0; i < start.Length; i++)
            {
                buf[0] = start[i];
                spi.Write(buf);
                while(dreq.Read() == false)
                    ;
            }

            //Play for 2 seconds
            Thread.Sleep(2000);

            //Write stop sequence
            for(var i = 0; i < end.Length; i++)
            {
                buf[0] = end[i];
                spi.Write(buf);
                while(dreq.Read() == false)
                    ;
            }
        }

        #endregion

        #region Playback

        private bool _stopPlayingRequested;
        private bool _isPlaying;

        /// <summary>
        /// Play samples from byte array async
        /// </summary>
        /// <param name="data"></param>
        public void Play(byte[] data)
        {
            _isPlaying = true;
            _stopPlayingRequested = false;

            playBackData = data;
            playbackThread = new Thread(PlayBackThreadFunction);
            playbackThread.Start();
        }

        private Stream playBackStream;

        public void Play(Stream stream)
        {
            _isPlaying = true;
            _stopPlayingRequested = false;
            playBackStream = stream;
            playbackThread = new Thread(PlayStreamTask);
            playbackThread.Start();
        }

        private void PlayStreamTask()
        {
            var block = new byte[32];

            spi.Config = dataConfig;

            while(true)
            {
                var bytesRead = playBackStream.Read(block, 0, 32);
                Debug.Print(bytesRead.ToString());
                if(bytesRead == 0 || _stopPlayingRequested)
                    break;

                while(dreq.Read() == false)
                    Thread.Sleep(10);

                spi.Write(block);
            }

            Reset();

            playBackData = null;
            _isPlaying = false;
        }

        private Stream _recordingStream;

        /// <summary>
        /// Playback thread function
        /// </summary>
        private void PlayBackThreadFunction()
        {
            var block = new byte[32];

            var size = playBackData.Length - playBackData.Length % 32;

            spi.Config = dataConfig;
            for(var i = 0; i < size; i += 32)
            {
                if(_stopPlayingRequested)
                    break;

                while(dreq.Read() == false)
                    Thread.Sleep(1); // wait till done

                Array.Copy(playBackData, i, block, 0, 32);

                spi.Write(block);
            }

            Reset();

            playBackData = null;
            _isPlaying = false;
        }

        public void StopPlaying()
        {
            _stopPlayingRequested = true;
        }

        #endregion

        private bool _isRecording;
        private bool _stopRecordingRequested;
        private ushort[] _oggPatch;

        private readonly byte[] _sampleBuffer = new byte[] {CMD_READ, SCI_HDAT0};
        private static readonly byte[] _recordingBuffer = new byte[1024];

        /// <summary>
        /// Optimized CommandRead for SCI_HDAT0
        /// </summary>
        /// <param name="nSamples"></param>
        private void ReadData(int nSamples)
        {
            var i = 0;
            spi.Config = cmdConfig;

            while(i < nSamples)
            {
                spi.WriteRead(_sampleBuffer, 0, 2, _recordingBuffer, i * 2, 2, 2);
                i++;
            }
        }


        /// <summary>
        /// Request recording to stop
        /// </summary>
        public void StopRecording()
        {
            if(!_stopRecordingRequested)
                _stopRecordingRequested = true;
        }

        #region Ogg Vorbis Recording

        public void RecordOggVorbis(Stream recordingStream, ushort[] oggPatch)
        {
            _isRecording = true;
            _stopRecordingRequested = false;
            _oggPatch = oggPatch;

            _recordingStream = recordingStream;
            recordingThread = new Thread(RecordOggVorbisThreadFunction);
            recordingThread.Start();
        }

        private void RecordOggVorbisThreadFunction()
        {
            Reset();

            SetVolume(255, 255);

            WriteRegister(SCI_CLOCKF, 0xc000);

            WriteRegister(SCI_BASS, 0x0000);
            WriteRegister(SCI_AIADDR, 0x0000);

            WriteRegister(SCI_WRAMADDR, 0xC01A);
            WriteRegister(SCI_WRAM, 0x0002);

            //Load Ogg Vorbis Encoder
            WritePatchFromArray(_oggPatch);

            WriteRegister(SCI_MODE, (ushort) (ReadRegister(SCI_MODE) | SM_ADPCM | SM_LINE1));
            WriteRegister(SCI_AICTRL1, 0);
            WriteRegister(SCI_AICTRL2, 4096);

            ////0x8000 - MONO
            ////0x8080 - STEREO
            WriteRegister(SCI_AICTRL0, 0x0000);

            WriteRegister(SCI_AICTRL3, 0);
            //CommandWrite(SCI_AICTRL3, 0x40);

            WriteRegister(SCI_AIADDR, 0x0034);

            while(dreq.Read() == false)
                ;

            var totalSamples = 0;

            var stopRecording = false;
            var stopRecordingRequestInProgress = false;

            var samples = 0;

            while(!stopRecording)
            {
                if(_stopRecordingRequested && !stopRecordingRequestInProgress)
                {
                    WriteRegister(SCI_AICTRL3, 0x0001);
                    _stopRecordingRequested = false;
                    stopRecordingRequestInProgress = true;
                }

                if(stopRecordingRequestInProgress)
                {
                    stopRecording = ((ReadRegister(SCI_AICTRL3) & 0x0002) != 0);
                }

                samples = ReadRegister(SCI_HDAT1);
                if(samples > 0)
                {
                    totalSamples = samples > 512 ? 512 : samples;

                    ReadData(totalSamples);
                    if(_recordingStream != null)
                        _recordingStream.Write(_recordingBuffer, 0, totalSamples << 1);

                    //Debug.Print("I have: " + samples.ToString() + " samples");
                }
                //Debug.Print("no data");
            }

            samples = ReadRegister(SCI_HDAT1);
            while(samples > 0)
            {
                totalSamples = samples > 512 ? 512 : samples;

                ReadData(totalSamples);
                if(_recordingStream != null)
                    _recordingStream.Write(_recordingBuffer, 0, totalSamples << 1);

                Debug.Print("I have: " + samples.ToString() + " samples");

                samples = ReadRegister(SCI_HDAT1);
            }

            if(_recordingStream != null)
            {
                _recordingStream.Close();
                _recordingStream = null;
            }

            Reset();

            _isRecording = false;
            _oggPatch = null;
        }

        #endregion
    }
}