namespace Librarian.KioskClient.Catalog.Models
{
    public class BookInfo
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string[] Authors { get; set; }
        public string ThumbnailUri { get; set; }
    }
}
