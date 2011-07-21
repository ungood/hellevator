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
using System.Diagnostics;

namespace Hellevator.Simulator.ViewModels
{
    public static class Stopwatch
    {
        private static DateTime lastReset = DateTime.Now;

        public static void Reset()
        {
            lastReset = DateTime.Now;
            Debug.Print("00:00.0000: Begin Stopwatch");
        }

        public static void Print(string format, params object[] args)
        {
            var current = DateTime.Now - lastReset;
            var interval = current.ToString("mm:ss.ffff");
            Debug.Print(interval + ": " + format, args);
        }
    }
}
