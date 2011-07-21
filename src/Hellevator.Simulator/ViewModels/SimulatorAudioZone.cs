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

using System.Collections.Generic;
using System.IO;
using System.Threading;
using Hellevator.Behavior.Interface;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Hellevator.Simulator.ViewModels
{
    internal class AudioMixer
    {
        public static AudioMixer Instance = new AudioMixer();

        private readonly IWavePlayer player;
        private readonly WaveMixerStream32 mixer;
        private readonly IDictionary<string, WaveStream> streams
            = new Dictionary<string,WaveStream>();

        private readonly object mixLock = new object();

        private AudioMixer()
        {
            mixer = new WaveMixerStream32 {
                AutoStop = false
            };
            player = new WasapiOut(AudioClientShareMode.Shared, 100);
            player.Init(mixer);
            player.Play();
        }

        public void Play(string zone, string filename, float pan)
        {
            Stopwatch.Print("{0} - {1}", zone, filename);

            if(!File.Exists(filename))
            {
                return;
            }

            lock(mixLock)
            {
                if(streams.ContainsKey(zone))
                    Stop(zone);

                var stream = new Mp3ConverterStream(filename, pan);
                streams.Add(zone, stream);
                mixer.AddInputStream(stream);
            }
        }

        public void Stop(string zone)
        {
            lock(mixLock)
            {
                WaveStream stream;
                if(!streams.TryGetValue(zone, out stream))
                    return;

                mixer.RemoveInputStream(stream);
                streams.Remove(zone);
            }
        }

        public void Close()
        {
            player.Stop();
            player.Dispose();
        }
    }

    internal class Mp3ConverterStream : WaveStream
    {
        private WaveStream sourceStream;

        public Mp3ConverterStream(string filename, float pan)
        {
            sourceStream = new Mp3FileReader(filename);
            var format = new WaveFormat(44100, 16, sourceStream.WaveFormat.Channels);

            if(sourceStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
            {
                sourceStream = WaveFormatConversionStream.CreatePcmStream(sourceStream);
                sourceStream = new BlockAlignReductionStream(sourceStream);
            }
            if(sourceStream.WaveFormat.SampleRate != 44100 ||
                sourceStream.WaveFormat.BitsPerSample != 16)
            {
                sourceStream = new WaveFormatConversionStream(format, sourceStream);
                sourceStream = new BlockAlignReductionStream(sourceStream);
            }

            sourceStream = new WaveChannel32(sourceStream, 1, pan);
        }

        public override WaveFormat WaveFormat
        {
            get { return sourceStream.WaveFormat; }
        }

        public override long Length
        {
            get { return sourceStream.Length; }
        }

        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return sourceStream.Read(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if(sourceStream != null)
            {
                sourceStream.Dispose();
                sourceStream = null;
            }
            base.Dispose(disposing);
        }
    }

    public class SimulatorAudioZone : IAudioZone
    {
        public string Name { get; private set; }
        public float Pan { get; private set; }

        public SimulatorAudioZone(string name, float pan)
        {
            Name = name;
            Pan = pan;
        }

        public WaitHandle Play(Playlist playlist)
        {
            AudioMixer.Instance.Play(Name, "Audio/" + playlist.GetNext() + ".mp3", Pan);
            return new AutoResetEvent(true);
        }

        public void Loop(Playlist playlist)
        {
            AudioMixer.Instance.Play(Name, "Audio/" + playlist.GetNext() + ".mp3", Pan);
        }

        WaitHandle IAudioZone.Stop()
        {
            AudioMixer.Instance.Stop(Name);
            return new AutoResetEvent(true);
        }
    }
}