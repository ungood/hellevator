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

using Hellevator.Behavior.Interface;

namespace Hellevator.Simulator.ViewModels
{
    public class SimulatorTextDisplay : ViewModelBase, ITextDisplay
    {
        private string topText;

        public string TopText
        {
            get { return topText; }
            set
            {
                if(value == topText)
                    return;

                topText = value;
                OnPropertyChanged("TopText");
            }
        }

        private string bottomText;

        public string BottomText
        {
            get { return bottomText; }
            set
            {
                if(value == bottomText)
                    return;

                bottomText = value;
                OnPropertyChanged("BottomText");
            }
        }

        public void Clear()
        {
            TopText = BottomText = "";
        }

        public void Print(int line, string format, params object[] args)
        {
            var text = string.Format(format, args);

            switch(line)
            {
                case 1:
                    TopText = text;
                    break;
                case 2:
                    BottomText = text;
                    break;
            }
        }
    }
}
