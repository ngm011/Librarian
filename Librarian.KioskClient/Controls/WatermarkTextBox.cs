using System.Windows;
using System.Windows.Controls;

namespace Librarian.KioskClient.Controls
{
    public class WatermarkTextBox : TextBox
    {
        static WatermarkTextBox() =>
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(WatermarkTextBox), new PropertyMetadata(""));

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
    }
}
