using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

using Microsoft.SPOT.Emulator;
using Microsoft.SPOT.Emulator.Gpio;

namespace HellevatorEmulator
{
    class Program : Emulator
    {
        public override void SetupComponent()
        {
            this.GpioPorts.MaxPorts = 128;

            GpioPort motorUp = new GpioPort();
            motorUp.ComponentId = "MotorUpButton";
            motorUp.Pin = (Microsoft.SPOT.Hardware.Cpu.Pin) 20;
            motorUp.ModesAllowed = GpioPortMode.InputPort;
            motorUp.ModesExpected = GpioPortMode.InputPort;

            GpioPort motorDown = new GpioPort();
            motorDown.ComponentId = "MotorDownButton";
            motorDown.Pin = (Microsoft.SPOT.Hardware.Cpu.Pin) 21;
            motorDown.ModesAllowed = GpioPortMode.InputPort;
            motorDown.ModesExpected = GpioPortMode.InputPort;

            this.RegisterComponent(motorUp);
            this.RegisterComponent(motorDown);

            base.SetupComponent();
        }

        public override void InitializeComponent()
        {
            base.InitializeComponent();

            // Start the UI in its own thread.
            Thread uiThread = new Thread(StartForm);
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();
        }

        public override void UninitializeComponent()
        {
            base.UninitializeComponent();

            // The emulator is stopped. Close the WinForm UI.
            Application.Exit();
        }

        private void StartForm()
        {
            // Some initial setup for the WinForm UI
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start the WinForm UI. Run() returns when the form is closed.
            Application.Run(new Form1(this));

            // When the user closes the WinForm UI, stop the emulator.
            Stop();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            (new Program()).Start();
        }
    }
}
