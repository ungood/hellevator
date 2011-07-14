using System;
using Microsoft.SPOT;

namespace Hellevator.Physical.Components
{
    public class AudioShieldRecorder
    {
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

        ///// <summary>
        ///// Returns true is shield is playing or recording audio
        ///// </summary>
        //public bool IsBusy
        //{
        //    get { return _isPlaying || _isRecording; }
        //}

        //private void WritePatchFromArray(ushort[] patch)
        //{
        //    spi.Config = cmdConfig;
        //    cmdBuffer[0] = 0x02;

        //    var count = 0;
        //    var i = 0;
        //    ushort value = 0;

        //    while(i < patch.Length)
        //    {
        //        cmdBuffer[1] = (byte) patch[i++];
        //        count = patch[i++];
        //        if((count & 0x8000) != 0)
        //        {
        //            count &= 0x7FFF;
        //            value = patch[i++];
        //            cmdBuffer[2] = (byte) (value >> 8);
        //            cmdBuffer[3] = (byte) value;

        //            while(count-- > 0)
        //            {
        //                spi.Write(cmdBuffer);
        //                while(dreq.Read() == false)
        //                    ;
        //            }
        //        }
        //        else
        //        {
        //            while(count-- > 0)
        //            {
        //                value = patch[i++];
        //                cmdBuffer[2] = (byte) (value >> 8);
        //                cmdBuffer[3] = (byte) value;
        //                spi.Write(cmdBuffer);
        //                while(dreq.Read() == false)
        //                    ;
        //            }
        //        }
        //    }
        //}

        //private void WritePatchFromStream(Stream s)
        //{
        //    spi.Config = cmdConfig;
        //    cmdBuffer[0] = 0x02;

        //    byte highByte = 0;
        //    byte lowByte = 0;
        //    var count = 0;
        //    while(true)
        //    {
        //        while(dreq.Read() == false)
        //            Thread.Sleep(1);

        //        //register address
        //        cmdBuffer[1] = (byte) s.ReadByte();
        //        s.ReadByte();
        //        lowByte = (byte) s.ReadByte();
        //        highByte = (byte) s.ReadByte();
        //        count = (highByte & 0x7F) << 8 | lowByte;
        //        if((highByte & 0x80) != 0)
        //        {
        //            cmdBuffer[3] = (byte) s.ReadByte();
        //            cmdBuffer[2] = (byte) s.ReadByte();
        //            while(count-- > 0)
        //            {
        //                spi.Write(cmdBuffer);
        //            }
        //        }
        //        else
        //        {
        //            while(count-- > 0)
        //            {
        //                cmdBuffer[3] = (byte) s.ReadByte();
        //                cmdBuffer[2] = (byte) s.ReadByte();
        //                spi.Write(cmdBuffer);
        //            }
        //        }
        //    }
        //}

        //#region VS1053b Tests

        ///// <summary>
        ///// Run sine test
        ///// </summary>
        //public void SineTest()
        //{
        //    byte[] buf = {0};

        //    WriteMode(Mode.SdiNew | Mode.Tests | Mode.Reset);

        //    byte[] start = {0x53, 0xEF, 0x6E, 0x7E, 0x00, 0x00, 0x00, 0x00};
        //    byte[] end = {0x45, 0x78, 0x69, 0x74, 0x00, 0x00, 0x00, 0x00};



        //    spi.Config = dataConfig;


        //    //Write start sequence
        //    for(var i = 0; i < start.Length; i++)
        //    {
        //        buf[0] = start[i];
        //        spi.Write(buf);
        //        while(dreq.Read() == false)
        //            ;
        //    }

        //    //Play for 2 seconds
        //    Thread.Sleep(2000);

        //    //Write stop sequence
        //    for(var i = 0; i < end.Length; i++)
        //    {
        //        buf[0] = end[i];
        //        spi.Write(buf);
        //        while(dreq.Read() == false)
        //            ;
        //    }
        //}

        //#endregion

        

        //private bool _isRecording;
        //private bool _stopRecordingRequested;
        //private ushort[] _oggPatch;

        //private readonly byte[] _sampleBuffer = new byte[] {CMD_READ, SCI_HDAT0};
        //private static readonly byte[] _recordingBuffer = new byte[1024];

        ///// <summary>
        ///// Optimized CommandRead for SCI_HDAT0
        ///// </summary>
        ///// <param name="nSamples"></param>
        //private void ReadData(int nSamples)
        //{
        //    var i = 0;
        //    spi.Config = cmdConfig;

        //    while(i < nSamples)
        //    {
        //        spi.WriteRead(_sampleBuffer, 0, 2, _recordingBuffer, i * 2, 2, 2);
        //        i++;
        //    }
        //}


        ///// <summary>
        ///// Request recording to stop
        ///// </summary>
        //public void StopRecording()
        //{
        //    if(!_stopRecordingRequested)
        //        _stopRecordingRequested = true;
        //}

        //#region Ogg Vorbis Recording

        //public void RecordOggVorbis(Stream recordingStream, ushort[] oggPatch)
        //{
        //    _isRecording = true;
        //    _stopRecordingRequested = false;
        //    _oggPatch = oggPatch;

        //    _recordingStream = recordingStream;
        //    recordingThread = new Thread(RecordOggVorbisThreadFunction);
        //    recordingThread.Start();
        //}

        //private void RecordOggVorbisThreadFunction()
        //{
        //    Reset();

        //    SetVolume(255, 255);

        //    WriteRegister(SCI_CLOCKF, 0xc000);

        //    WriteRegister(SCI_BASS, 0x0000);
        //    WriteRegister(SCI_AIADDR, 0x0000);

        //    WriteRegister(SCI_WRAMADDR, 0xC01A);
        //    WriteRegister(SCI_WRAM, 0x0002);

        //    //Load Ogg Vorbis Encoder
        //    WritePatchFromArray(_oggPatch);

        //    WriteRegister(SCI_MODE, (ushort) (ReadRegister(SCI_MODE) | SM_ADPCM | SM_LINE1));
        //    WriteRegister(SCI_AICTRL1, 0);
        //    WriteRegister(SCI_AICTRL2, 4096);

        //    ////0x8000 - MONO
        //    ////0x8080 - STEREO
        //    WriteRegister(SCI_AICTRL0, 0x0000);

        //    WriteRegister(SCI_AICTRL3, 0);
        //    //CommandWrite(SCI_AICTRL3, 0x40);

        //    WriteRegister(SCI_AIADDR, 0x0034);

        //    while(dreq.Read() == false)
        //        ;

        //    var totalSamples = 0;

        //    var stopRecording = false;
        //    var stopRecordingRequestInProgress = false;

        //    var samples = 0;

        //    while(!stopRecording)
        //    {
        //        if(_stopRecordingRequested && !stopRecordingRequestInProgress)
        //        {
        //            WriteRegister(SCI_AICTRL3, 0x0001);
        //            _stopRecordingRequested = false;
        //            stopRecordingRequestInProgress = true;
        //        }

        //        if(stopRecordingRequestInProgress)
        //        {
        //            stopRecording = ((ReadRegister(SCI_AICTRL3) & 0x0002) != 0);
        //        }

        //        samples = ReadRegister(SCI_HDAT1);
        //        if(samples > 0)
        //        {
        //            totalSamples = samples > 512 ? 512 : samples;

        //            ReadData(totalSamples);
        //            if(_recordingStream != null)
        //                _recordingStream.Write(_recordingBuffer, 0, totalSamples << 1);

        //            //Debug.Print("I have: " + samples.ToString() + " samples");
        //        }
        //        //Debug.Print("no data");
        //    }

        //    samples = ReadRegister(SCI_HDAT1);
        //    while(samples > 0)
        //    {
        //        totalSamples = samples > 512 ? 512 : samples;

        //        ReadData(totalSamples);
        //        if(_recordingStream != null)
        //            _recordingStream.Write(_recordingBuffer, 0, totalSamples << 1);

        //        Debug.Print("I have: " + samples.ToString() + " samples");

        //        samples = ReadRegister(SCI_HDAT1);
        //    }

        //    if(_recordingStream != null)
        //    {
        //        _recordingStream.Close();
        //        _recordingStream = null;
        //    }

        //    Reset();

        //    _isRecording = false;
        //    _oggPatch = null;
        //}

        //#endregion
    }
}
