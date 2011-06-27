using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Hellevator.Simulator.Data
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Hellevator.Behavior.Animations.Color) value;
            if(color == null)
                return Colors.Black;
            var mediaColor = Color.FromRgb(color.Red, color.Green, color.Blue);
            return mediaColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
