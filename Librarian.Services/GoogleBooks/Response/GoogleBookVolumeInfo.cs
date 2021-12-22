namespace Librarian.Services.GoogleBooks.Response
{
    internal class GoogleBookVolumeInfo 
    {
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Publisher { get; set; }
        public string? PublishedDate { get; set; }
        public GoogleBookIndustryIdentifier[]? IndustryIdentifiers { get; set; }
        public string[]? Authors { get; set; }
        public GoogleBookImageLink? ImageLinks { get; set; }
    }
}
