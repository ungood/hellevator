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

using System.Collections;
using System.Reflection;

namespace Hellevator.Behavior.Interface
{
    public class Playlist
    {
        public bool Shuffle { get; private set; }

        public int Index { get; private set; }
        
        private readonly string[] filenames;
        private int current;
        
        public Playlist(bool shuffle, params string[] filenames)
        {
            Shuffle = shuffle;
            this.filenames = filenames;
            
            Reset();
        }

        public string GetNext()
        {
            if(current >= filenames.Length)
                Reset();
            
            return filenames[current++];
        }

        private void Reset()
        {
            current = 0;
            if(!Shuffle)
                return;

            for(var i = 1; i < filenames.Length; i++)
            {
                var swap = RNG.Next(i);
                var temp = filenames[i];
                filenames[i] = filenames[swap];
                filenames[swap] = temp;
            }
        }

        public static readonly Playlist Beep = new Playlist(true, "beep0", "beep1");
        public static readonly Playlist Ding = new Playlist(true, "ding0", "ding1");
        public static readonly Playlist ElevatorMusic = new Playlist(true, "sample", "backinblack");
        public static readonly Playlist WarmupSounds = new Playlist(true, "blah");
        public static readonly Playlist IdleSounds = new Playlist(true, "blah");

        public static readonly Playlist WelcomeToHellevator = new Playlist(false, "welcome");

        private static readonly Hashtable Playlists = new Hashtable();

        static Playlist()
        {
            var type = typeof(Playlist);
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            for(var i = 0; i < fields.Length; i++)
            {
                var playlist = fields[i].GetValue(null) as Playlist;
                if(playlist == null)
                    continue;

                playlist.Index = i;
                Playlists.Add(i, playlist);
            }
        }

        public static Playlist GetPlaylist(int index)
        {
            return (Playlist)Playlists[index];
        }
    }
}