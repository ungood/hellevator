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

namespace Hellevator.Behavior.Interface
{
    public enum Location
    {
        Unknown         = 0,
        Hell1,
        Hell2,
        TopFloor,
        Heaven,
        Entrance,
        MidPurgatory,
        Purgatory,
        BlackRockCity
    }

    public static class LocationExtension
    {
        public static string GetName(this Location location)
        {
            switch(location)
            {
                case Location.Hell1:
                case Location.Hell2:
                    return "HELL";
                case Location.TopFloor:
                    return "TOP FLOOR";
                case Location.Heaven:
                    return "HEAVEN";
                case Location.Entrance:
                case Location.BlackRockCity:
                    return "BRC";
                case Location.Purgatory:
                case Location.MidPurgatory:
                    return "NOWHERE";
                default:
                    return "UNKNOWN";
            }
        }

        public static int GetFloor(this Location location)
        {
            switch(location)
            {
                case Location.TopFloor:
                    return 24;
                case Location.Heaven:
                    return 88;
                case Location.MidPurgatory:
                    return 13;
                case Location.Purgatory:
                    return 1;
                case Location.Hell1:
                    return -12;
                case Location.Hell2:
                    return -100;
                default:
                    return 1;
            }
        }
    }
}