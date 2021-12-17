namespace Librarian.Data
{
    public class CatalogUsagePrintDto
    {
        public int Id { get; set; }
        public string? OriginatingIP { get; set; }
        public string? OriginatingHost { get; set; }
        public string? Query { get; set; }
        public int ResultCount { get; set; }
    }
}
