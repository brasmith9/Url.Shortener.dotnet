using Microsoft.EntityFrameworkCore;

namespace Url.Shortner.Entity;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).IsRequired();
            
            entity.Property(e => e.Url).IsRequired();
            
            entity.Property(e => e.ShortUrl).IsRequired();
            
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(7);
            
            entity.HasIndex(e => e.Code)
                .IsUnique();
        });
        modelBuilder.Entity<ShortenedUrl>().Property(x => x.Url).IsRequired();
        modelBuilder.Entity<ShortenedUrl>().Property(x => x.Code).IsRequired().HasMaxLength(7);
        
        modelBuilder.Entity<ShortenedUrl>().HasIndex(x => x.Code).IsUnique();
    }
    
}