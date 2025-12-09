
using Microsoft.EntityFrameworkCore;
using ProductPrediction.API.Entities;

namespace ProductPrediction.API.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // DbSets = tabelas
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<ShoppingList> ShoppingLists => Set<ShoppingList>();
    public DbSet<User> User => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeamento via Fluent API (opcional se usar convenções)
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users"); // nome da tabela

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(200);
            entity.Property(p => p.Email)
                  .IsRequired()
                  .HasMaxLength(200);

        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.ToTable("purchases");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Date)
                  .IsRequired();
            entity.Property(p => p.Items)
                  .IsRequired();
            // Relacionamento com User
            entity.HasOne(p => p.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ShoppingList>(entity =>
        {
            entity.ToTable("shopping_lists");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Items)
                  .IsRequired();

            entity.HasOne(s => s.User)
                .WithMany(u => u.ShoppingLists)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
