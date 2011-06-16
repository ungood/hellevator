using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SPOT.Emulator;
using Microsoft.SPOT.Emulator.Gpio;

namespace HellevatorEmulator
{
    public partial class Form1 : Form
    {
        private Emulator emulator;
        private GpioPort motorUpButtonPort;
        private GpioPort motorDownButtonPort;

        public Form1(Emulator emulator)
        {
            this.emulator = emulator;

            motorUpButtonPort =
                this.emulator.FindComponentById("MotorUpButton") as GpioPort;
            motorDownButtonPort =
                this.emulator.FindComponentById("MotorDownButton") as GpioPort;

            InitializeComponent();
        }

        delegate void GpioPortWriteDelegate(bool state);

        private void GpioPortWrite(GpioPort port, bool value)
        {
            port.Invoke(new GpioPortWriteDelegate(port.Write), value);
        }

        private void motorUpButton_MouseUp(object sender, MouseEventArgs e)
        {
            GpioPortWrite(motorUpButtonPort, true);
        }

        private void motorUpButton_MouseDown(object sender, MouseEventArgs e)
        {
            GpioPortWrite(motorUpButtonPort, false);
        }

        private void motorDownButton_MouseDown(object sender, MouseEventArgs e)
        {
            GpioPortWrite(motorDownButtonPort, false);
        }

        private void motorDownButton_MouseUp(object sender, MouseEventArgs e)
        {
            GpioPortWrite(motorDownButtonPort, true);
        }
    }
}
