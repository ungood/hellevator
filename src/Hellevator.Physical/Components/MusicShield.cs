using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.IO;
 
namespace GHIElectronics.NETMF.FEZ.Shields
{
    public partial class MusicShield
    {
        /// <summary>
        /// Playback thread
        /// </summary>
        private Thread _playbackThread;
 
        /// <summary>
        /// Recording thread
        /// </summary>
        private Thread _recordingThread;
 
        // Some GPIO pins
        private InputPort _dreq;
 
        // Define SPI Configuration for MP3 decoder
        SPI _spi;
        private SPI.Configuration _dataConfig;
        private SPI.Configuration _cmdConfig;
 
        const byte CMD_WRITE = 0x02;
 
        const byte CMD_READ = 0x03;
 
        #region SCI_MODE bits
 
        // Values
        const ushort SM_RESET = 0x04;
        const ushort SM_CANCEL = 0x10;
        const ushort SM_TESTS = 0x20;
        const ushort SM_SDINEW = 0x800;
        const ushort SM_ADPCM = 0x1000;
        const ushort SM_LINE1 = 0x4000;
 
        #endregion
 
        #region Registers
 
        /// <summary>
        /// Mode control
        /// R/W
        /// </summary>
        const int SCI_MODE = 0x00;
 
        /// <summary>
        /// Status of VS1053b
        /// R/W
        /// </summary>
        const int SCI_STATUS = 0x01;
 
        /// <summary>
        /// Built-in bass/treble control
        /// R/W
        /// </summary>
        const int SCI_BASS = 0x02;
 
        /// <summary>
        /// Clock freq + multiplier
        /// R/W
        /// </summary>
        const int SCI_CLOCKF = 0x03;
 
        /// <summary>
        /// Volume control
        /// R/W
        /// </summary>
        const int SCI_WRAM = 0x06;
 
        /// <summary>
        /// Volume control
        /// R/W
        /// </summary>
        const int SCI_WRAMADDR = 0x07;
 
        /// <summary>
        /// Stream header data 0
        /// R
        /// </summary>
        const int SCI_HDAT0 = 0x08;
 
        /// <summary>
        /// Stream header data 1
        /// R
        /// </summary>
        const int SCI_HDAT1 = 0x09;
 
        /// <summary>
        /// Volume control
        /// R/W
        /// </summary>
        const int SCI_AIADDR = 0x0A;
 
        /// <summary>
        /// Volume control
        /// R/W
        /// </summary>
        const int SCI_VOL = 0x0B;
 
        /// <summary>
        /// Application control register 0
        /// R/W
        /// </summary>
        const int SCI_AICTRL0 = 0x0C;
 
        /// <summary>
        /// Application control register 1
        /// R/W
        /// </summary>
        const int SCI_AICTRL1 = 0x0D;
 
        /// <summary>
        /// Application control register 2
        /// R/W
        /// </summary>
        const int SCI_AICTRL2 = 0x0E;
 
        /// <summary>
        /// Application control register 3
        /// R/W
        /// </summary>
        const int SCI_AICTRL3 = 0x0F;
 
 
        #endregion
 
        #region Recording constants
 
        const int PCM_MODE_JOINTSTEREO = 0x00;
        const int PCM_MODE_DUALCHANNEL = 0x01;
        const int PCM_MODE_LEFTCHANNEL = 0x02;
        const int PCM_MODE_RIGHTCHANNEL = 0x03;
 
        const int PCM_ENC_ADPCM = 0x00;
        const int PCM_ENC_PCM = 0x04;
 
        #endregion
 
        /// <summary>
        /// Playback buffer
        /// </summary>
        private byte[] _playBackData = null;
 
        /// <summary>
        /// Command buffer
        /// </summary>
        private byte[] cmdBuffer = new byte[4];
 
        public MusicShield(SPI.SPI_module spi, FEZ_Pin.Digital dataCS, FEZ_Pin.Digital cmdCS, FEZ_Pin.Digital DREQ)
        {
            _dataConfig = new SPI.Configuration((Cpu.Pin)dataCS, false, 0, 0, false, true, 2000, spi); //, (Cpu.Pin)DREQ, false);
            _cmdConfig = new SPI.Configuration((Cpu.Pin)cmdCS, false, 0, 0, false, true, 2000, spi); // , (Cpu.Pin)DREQ, false);
            _dreq = new InputPort((Cpu.Pin)DREQ, false, Port.ResistorMode.PullUp);
 
            _spi = new SPI(_dataConfig);
 
            Reset();
 
            CommandWrite(SCI_MODE, SM_SDINEW);
            CommandWrite(SCI_CLOCKF, 0xa000);
            CommandWrite(SCI_VOL, 0x0101);  // highest volume -1
 
            if (CommandRead(SCI_VOL) != (0x0101))
            {
                throw new Exception("Failed to initialize MP3 Decoder.");
            }
        }
 
        #region Command Register operations
 
        /// <summary>
        /// Reads 16bit value from a register
        /// </summary>
        /// <param name="register">Source register</param>
        /// <returns>16bit value from the source register</returns>
        private ushort CommandRead(byte register)
        {
            ushort temp;
 
            while (_dreq.Read() == false)
                Thread.Sleep(1);
 
            _spi.Config = _cmdConfig;
            cmdBuffer[0] = CMD_READ;
 
            cmdBuffer[1] = register;
            cmdBuffer[2] = 0;
            cmdBuffer[3] = 0;
 
            _spi.WriteRead(cmdBuffer, cmdBuffer, 2);
 
            temp = cmdBuffer[0];
            temp <<= 8;
 
            temp += cmdBuffer[1];
 
            return temp;
        }
 
        /// <summary>
        /// Writes 16bit value to a register
        /// </summary>
        /// <param name="register">target register</param>
        /// <param name="data">data to write</param>
        private void CommandWrite(byte register, ushort data)
        {
            while (_dreq.Read() == false)
                ;
 
            _spi.Config = _cmdConfig;
            cmdBuffer[0] = CMD_WRITE;
 
            cmdBuffer[1] = register;
            cmdBuffer[2] = (byte)(data >> 8);
            cmdBuffer[3] = (byte)data;
 
            _spi.Write(cmdBuffer);
 
        }
 
        #endregion
 
        /// <summary>
        /// Performs soft reset
        /// </summary>
        private void Reset()
        {
            CommandWrite(SCI_MODE, SM_SDINEW | SM_RESET);
 
            Thread.Sleep(1);
 
            CommandWrite(SCI_CLOCKF, 0xa000);
 
            while (_dreq.Read() == false)
                ;
        }
 
        /// <summary>
        /// Set volume for both channels. Valid values 0-255
        /// </summary>
        /// <param name="leftChannelVolume">0 - silence, 255 - loudest</param>
        /// <param name="rightChannelVolume">0 - silence, 255 - loudest</param>
        public void SetVolume(byte leftChannelVolume, byte rightChannelVolume)
        {
            CommandWrite(SCI_VOL, (ushort)((255 - leftChannelVolume) << 8 | (255 - rightChannelVolume)));
        }
 
        /// <summary>
        /// Returns true is shield is playing or recording audio
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isPlaying || _isRecording;
            }
        }
 
        private void WritePatchFromArray(ushort[] patch)
        {
            _spi.Config = _cmdConfig;
            cmdBuffer[0] = 0x02;
 
            int count = 0;
            int i = 0;
            ushort value = 0;
 
            while (i < patch.Length)
            {
 
                cmdBuffer[1] = (byte)patch[i++];
                count = patch[i++];
                if ((count & 0x8000) != 0)
                {
                    count &= 0x7FFF;
                    value = patch[i++];
                    cmdBuffer[2] = (byte)(value >> 8);
                    cmdBuffer[3] = (byte)value;
 
                    while (count-- > 0)
                    {
                        _spi.Write(cmdBuffer);
                        while (_dreq.Read() == false)
                            ;
                    }
                }
                else
                {
                    while (count-- > 0)
                    {
                        value = patch[i++];
                        cmdBuffer[2] = (byte)(value >> 8);
                        cmdBuffer[3] = (byte)value;
                        _spi.Write(cmdBuffer);
                        while (_dreq.Read() == false)
                            ;
                    }
 
                }
            }
        }
 
        private void WritePatchFromStream(Stream s)
        {
            _spi.Config = _cmdConfig;
            cmdBuffer[0] = 0x02;
 
            byte highByte = 0;
            byte lowByte = 0;
            int count = 0;
            while (true)
            {
                while (_dreq.Read() == false)
                    Thread.Sleep(1);
 
                //register address
                cmdBuffer[1] = (byte)s.ReadByte();
                s.ReadByte();
                lowByte = (byte)s.ReadByte();
                highByte = (byte)s.ReadByte();
                count = (highByte & 0x7F) << 8 | lowByte;
                if ((highByte & 0x80) != 0)
                {
                    cmdBuffer[3] = (byte)s.ReadByte();
                    cmdBuffer[2] = (byte)s.ReadByte();
                    while (count-- > 0)
                    {
                        _spi.Write(cmdBuffer);
                    }
                }
                else
                {
                    while (count-- > 0)
                    {
                        cmdBuffer[3] = (byte)s.ReadByte();
                        cmdBuffer[2] = (byte)s.ReadByte();
                        _spi.Write(cmdBuffer);
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
            byte[] buf = { 0 };
 
            CommandWrite(SCI_MODE, SM_SDINEW | SM_TESTS | SM_RESET);
 
            byte[] start = { 0x53, 0xEF, 0x6E, 0x7E, 0x00, 0x00, 0x00, 0x00 };
            byte[] end = { 0x45, 0x78, 0x69, 0x74, 0x00, 0x00, 0x00, 0x00 };
 
            _spi.Config = _dataConfig;
 
 
            //Write start sequence
            for (int i = 0; i < start.Length; i++)
            {
                buf[0] = start[i];
                _spi.Write(buf);
                while (_dreq.Read() == false)
                    ;
            }
 
            //Play for 2 seconds
            Thread.Sleep(2000);
 
            //Write stop sequence
            for (int i = 0; i < end.Length; i++)
            {
                buf[0] = end[i];
                _spi.Write(buf);
                while (_dreq.Read() == false)
                    ;
            }
        }
 
        #endregion
 
        #region Playback
 
        bool _stopPlayingRequested = false;
        bool _isPlaying = false;
 
        /// <summary>
        /// Play samples from byte array async
        /// </summary>
        /// <param name="data"></param>
        public void Play(byte[] data)
        {
            _isPlaying = true;
            _stopPlayingRequested = false;
 
            _playBackData = data;
            _playbackThread = new Thread(new ThreadStart(this.PlayBackThreadFunction));
            _playbackThread.Start();
        }
 
        Stream _recordingStream;

        public void Write(byte[] data)
        {
            while(_dreq.Read() == false)
                Thread.Sleep(1);
            _spi.Config = _dataConfig;
            _spi.Write(data);
        }
 
        /// <summary>
        /// Playback thread function
        /// </summary>
        private void PlayBackThreadFunction()
        {
            byte[] block = new byte[32];
 
            int size = _playBackData.Length - _playBackData.Length % 32;
 
            _spi.Config = _dataConfig;
            for (int i = 0; i < size; i += 32)
            {
                if (_stopPlayingRequested)
                    break;
 
                while (_dreq.Read() == false)
                    Thread.Sleep(1);  // wait till done
 
                Array.Copy(_playBackData, i, block, 0, 32);
 
                _spi.Write(block);
            }
 
            Reset();
 
            _playBackData = null;
            _isPlaying = false;
        }
 
        public void StopPlaying()
        {
            _stopPlayingRequested = true;
        }
 
        #endregion
 
 
        bool _isRecording = false;
        bool _stopRecordingRequested = false;
        ushort[] _oggPatch;
 
        private byte[] _sampleBuffer = new byte[] { CMD_READ, SCI_HDAT0 };
        static byte[] _recordingBuffer = new byte[1024];
 
        /// <summary>
        /// Optimized CommandRead for SCI_HDAT0
        /// </summary>
        /// <param name="nSamples"></param>
        private void ReadData(int nSamples)
        {
            int i = 0;
            _spi.Config = _cmdConfig;
 
            while (i < nSamples)
            {
                _spi.WriteRead(_sampleBuffer, 0, 2, _recordingBuffer, i * 2, 2, 2);
                i++;
            }
        }
 
 
        /// <summary>
        /// Request recording to stop
        /// </summary>
        public void StopRecording()
        {
            if (!_stopRecordingRequested)
                _stopRecordingRequested = true;
        }
 
        #region Ogg Vorbis Recording
 
        public void RecordOggVorbis(Stream recordingStream, ushort[] oggPatch)
        {
            _isRecording = true;
            _stopRecordingRequested = false;
            _oggPatch = oggPatch;
 
            _recordingStream = recordingStream;
            _recordingThread = new Thread(new ThreadStart(this.RecordOggVorbisThreadFunction));
            _recordingThread.Start();
        }
 
        private void RecordOggVorbisThreadFunction()
        {
            Reset();
 
            SetVolume(255, 255);
 
            CommandWrite(SCI_CLOCKF, 0xc000);
 
            CommandWrite(SCI_BASS, 0x0000);
            CommandWrite(SCI_AIADDR, 0x0000);
 
            CommandWrite(SCI_WRAMADDR, 0xC01A);
            CommandWrite(SCI_WRAM, 0x0002);
 
            //Load Ogg Vorbis Encoder
            WritePatchFromArray(_oggPatch);
 
            CommandWrite(SCI_MODE, (ushort)(CommandRead(SCI_MODE) | SM_ADPCM | SM_LINE1));
            CommandWrite(SCI_AICTRL1, 0);
            CommandWrite(SCI_AICTRL2, 4096);
 
            ////0x8000 - MONO
            ////0x8080 - STEREO
            CommandWrite(SCI_AICTRL0, 0x0000);
 
            CommandWrite(SCI_AICTRL3, 0);
            //CommandWrite(SCI_AICTRL3, 0x40);
 
            CommandWrite(SCI_AIADDR, 0x0034);
 
            while (_dreq.Read() == false)
                ;
 
            int totalSamples = 0;
 
            bool stopRecording = false;
            bool stopRecordingRequestInProgress = false;
 
            int samples = 0;
 
            while (!stopRecording)
            {
                if (_stopRecordingRequested && !stopRecordingRequestInProgress)
                {
                    CommandWrite(SCI_AICTRL3, 0x0001);
                    _stopRecordingRequested = false;
                    stopRecordingRequestInProgress = true;
                }
 
                if (stopRecordingRequestInProgress)
                {
                    stopRecording = ((CommandRead(SCI_AICTRL3) & 0x0002) != 0);
                }
 
                samples = CommandRead(SCI_HDAT1);
                if (samples > 0)
                {
                    totalSamples = samples > 512 ? 512 : samples;
 
                    ReadData(totalSamples);
                    if (_recordingStream != null)
                        _recordingStream.Write(_recordingBuffer, 0, totalSamples << 1);
 
                    //Debug.Print("I have: " + samples.ToString() + " samples");
                }
                //Debug.Print("no data");
            }
 
            samples = CommandRead(SCI_HDAT1);
            while (samples > 0)
            {
                totalSamples = samples > 512 ? 512 : samples;
 
                ReadData(totalSamples);
                if (_recordingStream != null)
                    _recordingStream.Write(_recordingBuffer, 0, totalSamples << 1);
 
                Debug.Print("I have: " + samples.ToString() + " samples");
 
                samples = CommandRead(SCI_HDAT1);
            }
 
            if (_recordingStream != null)
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