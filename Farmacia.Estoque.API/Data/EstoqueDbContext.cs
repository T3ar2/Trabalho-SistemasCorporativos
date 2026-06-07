using Farmacia.Estoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Farmacia.Estoque.API.Data
{
    public class EstoqueDbContext : DbContext
    {
        public EstoqueDbContext(DbContextOptions<EstoqueDbContext> options) : base(options) { }

        public DbSet<LoteEstoque> Lotes { get; set; }
    }
}