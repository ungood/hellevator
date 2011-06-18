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

namespace Hellevator.Behavior
{
    public enum Destination
    {
        Unknown         = 0,
        Hell            = 1,
        Heaven          = 2,
        Entrance        = 2,
        Purgatory       = 3,
        BlackRockCity   = 4,
    }

    public static class Floors
    {
        public const float BlackRockCity = 7;
    }
}
