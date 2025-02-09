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
namespace Hellevator.Physical
{
    public static class BitExtensions
    {
        private static readonly byte[] BitMask = new byte[] {1, 2, 4, 8, 16, 32, 64, 128};

        public static byte Set(this byte b, int bit)
        {
            return (byte)(b | BitMask[bit]);
        }

        public static byte Clear(this byte b, int bit)
        {
            return (byte) (b & ~BitMask[bit]);
        }

        public static byte Toggle(this byte b, int bit)
        {
            return (byte) (b ^ BitMask[bit]);
        }

        public static bool IsSet(this byte b, int bit)
        {
            return (b & (1 << bit)) != 0;
        }
    }
}
