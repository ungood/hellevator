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

namespace Hellevator.Behavior.Scenarios
{
    public class RandomScenario : Scenario
    {
        public static readonly RandomScenario Instance = new RandomScenario();
        
        private readonly Random rand;

        public override string Name
        {
            get { return "RANDOM"; }
        }

        public RandomScenario()
        {
            rand = new Random();
        }

        public override void Run()
        {
            var random = GetRandom();
            Hellevator.Debug.Print(1, "RND: " + random.Name);
            random.Run();
        }

        private Scenario GetRandom()
        {
            switch(rand.Next(6))
            {
                case 0:
                    return HeavenScenario.Instance;
                case 1:
                    return HeavenHellScenario.Instance;
                case 2:
                case 3:
                    return PurgatoryScenario.Instance;
                case 4:
                case 5:
                    return PurgatoryScenario.Instance;
                default: // Shouldn't get here, but if we do, try again!
                    return Instance;
            }
        }
    }
}