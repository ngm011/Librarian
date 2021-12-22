using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Librarian.KioskClient.MvvmInfrastructure.Converters
{
    public class BooleanToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public Visibility NullValue { get; set; } = Visibility.Collapsed;
        public Visibility NotNullValue { get; set; } = Visibility.Visible;
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        public bool IsNegated { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return this.NullValue;

            if (!bool.TryParse(value.ToString(), out bool boolValue))
                return this.NotNullValue;

            return boolValue ^ this.IsNegated ? this.TrueValue : this.FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is Visibility visibility ?
                visibility == TrueValue ?
                    true : visibility == FalseValue ? false : throw new InvalidOperationException("Target has to be specified") :
                throw new InvalidOperationException("Target type has to be Visibility");

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
