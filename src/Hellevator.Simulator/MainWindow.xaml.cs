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

using System.Threading.Tasks;
using System.Windows;
using Hellevator.Behavior;
using Hellevator.Simulator.ViewModels;

namespace Hellevator.Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += HandleLoaded;
            Closing += HandleClosing;
        }

        private HellevatorSimulator simulator;

        void HandleClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AudioMixer.Instance.Close();
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            simulator = new HellevatorSimulator();
            DataContext = simulator;
            Task.Factory.StartNew(() => Script.Run(simulator),
                TaskCreationOptions.LongRunning);
        }
    }
}