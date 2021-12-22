using Librarian.KioskClient.Catalog.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Librarian.KioskClient.Catalog.Converters
{
    public class SearchToTitleConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not SearchType searchType) throw new InvalidOperationException("Source has to be of SearchType");

            switch (searchType)
            {
                case SearchType.ByAuthor:
                    return "BY AUTHOR";
                case SearchType.ByTitle:
                    return "BY TITLE";
                default:
                    throw new InvalidOperationException("Provided SearchType is not supported");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
