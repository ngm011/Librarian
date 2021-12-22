using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Librarian.KioskClient.Catalog.Validation
{
    public class SearchKeywordRule : ValidationRule
    {
        public int MaxLength { get; set; } = 50;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is not string txt)
                return new ValidationResult(false, "Illegal search keyword");

            if (txt.Trim().Length > this.MaxLength)
                return new ValidationResult(false, "Search keyword cannot be more than 50 characters long");

            if (txt.All(c => Char.IsDigit(c)))
                return new ValidationResult(false, "Search keyword must have alphabetical characters");


            return ValidationResult.ValidResult;
        }
    }
}
