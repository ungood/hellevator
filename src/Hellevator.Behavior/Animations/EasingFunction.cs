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

namespace Hellevator.Behavior.Animations
{
    public enum EasingMode
    {
        InOut, In, Out
    }

    public abstract class EasingFunction
    {
        public EasingMode Mode { get; set; }

        protected abstract double EaseIn(double time);

        /// <summary>
        /// The time/progress should be between 0.0 and 1.0
        /// </summary>
        /// <returns>
        /// The modified (eased) time
        /// </returns>
        public double Ease(double time)
        {
            switch (Mode)
            {
                case EasingMode.In:
                    return EaseIn(time); 
                case EasingMode.Out:
                    // EaseOut is the same as EaseIn, except time is reversed & the result is flipped. 
                    return 1.0 - EaseIn(1.0 - time); 
                default: 
                    // EaseInOut is a combination of EaseIn & EaseOut fit to the 0-1, 0-1 range.
                    return (time < 0.5) ?
                        EaseIn(time * 2.0 ) * 0.5 :
                        (1.0 - EaseIn((1.0 - time) * 2.0)) * 0.5 + 0.5; 
            }
        }        
    }
}