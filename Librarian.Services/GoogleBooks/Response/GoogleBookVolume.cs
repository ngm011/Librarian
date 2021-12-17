namespace Librarian.Services.GoogleBooks.Response
{
    internal class GoogleBookVolume 
    { 
        public string? Id { get; set; }
        public string? Etag { get; set; }
        public string? SelfLink { get; set; }
        public GoogleBookVolumeInfo? VolumeInfo { get; set; }
    }
}
