using Farmacia.Catalogo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Farmacia.Catalogo.API.Data
{
    public class CatalogoDbContext : DbContext
    {
        public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Categoria)
                .HasConversion<string>();

            modelBuilder.Entity<Produto>()
                .Property(p => p.PrecoBase)
                .HasColumnType("decimal(10,2)");
        }
    }
}