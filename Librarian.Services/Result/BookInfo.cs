namespace Librarian.Services.Result
{
    public class BookInfo
    {
        internal BookInfo(
            string id,
            string identifier,
            string title,
            string subtitle,
            string publisher,
            string publishedDate,
            string[] authors,
            string thumbnailUri)
        {
            Id = id;
            Identifier = identifier;
            Title = title;
            Subtitle = subtitle;
            Publisher = publisher;
            PublishedDate = publishedDate;
            Authors = authors;
            ThumbnailUri = thumbnailUri;
        }

        public string Id { get; }
        public string Identifier { get; }
        public string Title { get; }
        public string Subtitle { get; }
        public string Publisher { get; }
        public string PublishedDate { get; }
        public string[] Authors { get; }
        public string ThumbnailUri { get; }
    }
}
