using Microsoft.EntityFrameworkCore;

namespace Librarian.Data
{
    public class CatalogContext : DbContext
    {
        private readonly string _connectionString;

        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) =>
            _connectionString = "";

        public DbSet<CatalogUsagePrintDto>? UsagePrints { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null) return;

            if (!optionsBuilder.IsConfigured && _connectionString.Length > 0)
                optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) return;

            modelBuilder.Entity<CatalogUsagePrintDto>().ToTable("CatalogUsagePrints");
            modelBuilder.Entity<CatalogUsagePrintDto>()
                .Property(dto => dto.OriginatingIP)
                .HasMaxLength(20)
                .IsUnicode(false);
            modelBuilder.Entity<CatalogUsagePrintDto>()
                .Property(dto => dto.OriginatingHost)
                .HasMaxLength(256);
        }
    }
}
