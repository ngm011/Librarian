namespace Librarian.Services.GoogleBooks.Response
{
    internal class GoogleBookResponse
    { 
        public string? Kind { get; set; }
        public int TotalItems { get; set; }

        public GoogleBookVolume[]? Items { get; set; }
    }
}
