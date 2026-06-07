using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Farmacia.Estoque.API.Data
{
    public class EstoqueDbContextFactory : IDesignTimeDbContextFactory<EstoqueDbContext>
    {
        public EstoqueDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EstoqueDbContext>();
            var connectionString = "Server=localhost;Database=FarmaciaEstoqueDb;Uid=root;Pwd=;";
            
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)));

            return new EstoqueDbContext(optionsBuilder.Options);
        }
    }
}