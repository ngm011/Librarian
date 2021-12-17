using System.ComponentModel.DataAnnotations;

namespace Librarian.ApiPortal.Models
{
    public class SearchRequest
    {
        [StringLength(50), NoNumberOnlyValue(ErrorMessage = "Number only Title Term is not allowed")]
        public string TitleTerm { get; set; }
        [StringLength(50), NoNumberOnlyValue(ErrorMessage = "Number only Author Term is not allowed")]
        public string AuthorTerm { get; set; }
    }
}
