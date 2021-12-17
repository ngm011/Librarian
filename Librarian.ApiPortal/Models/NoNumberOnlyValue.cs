using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Librarian.ApiPortal.Models
{
    public class NoNumberOnlyValue : ValidationAttribute
    {
        public override bool IsValid(object value) =>
            !value?.ToString()?.ToCharArray()?.All(c => 
                int.TryParse(c.ToString(), out _)) ?? true;
    }
}
