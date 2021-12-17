namespace Librarian.Services.Result
{
    public class BookInfo
    {
        internal BookInfo(
            string id, 
            string title, 
            string subtitle, 
            string[] authors, 
            string thumbnailUri) 
        {
            Id = id;
            Title = title;
            Subtitle = subtitle;
            Authors = authors;
            ThumbnailUri = thumbnailUri;
        }

        public string Id { get; }
        public string Title { get; }
        public string Subtitle { get; }
        public string[] Authors { get; }
        public string ThumbnailUri { get; }
    }
}
