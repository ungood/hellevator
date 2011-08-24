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

using System.Threading;
using Microsoft.SPOT.Hardware;

namespace Hellevator.Physical.Components
{
    public abstract class SpiWriter
    {
        protected const int BufferSize = 32;

        protected SPI.Configuration Configuration { get; private set; }
        
        protected InputPort DataRequest { get; private set; }

        public ManualResetEvent BusyEvent { get; private set; }

        protected SpiWriter(SPI.Configuration configuration, InputPort dreq)
        {
            Configuration = configuration;
            DataRequest = dreq;
            BusyEvent = new ManualResetEvent(true);
        }

        protected abstract byte[] GetData();

        public void WriteInternal(SPI spi)
        {
            var data = GetData();
            if(data == null)
                return;

            spi.Config = Configuration;
            spi.Write(data);
        }
    }
}
