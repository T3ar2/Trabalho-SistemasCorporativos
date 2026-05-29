using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Farmacia.Catalogo.API.Data
{
    public class CatalogoDbContextFactory : IDesignTimeDbContextFactory<CatalogoDbContext>
    {
        public CatalogoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogoDbContext>();
            var connectionString = "Server=localhost;Database=FarmaciaCatalogoDb;Uid=root;Pwd=;";
            
            optionsBuilder.UseMySql(
                connectionString, 
                new MySqlServerVersion(new Version(8, 0, 30))
            );

            return new CatalogoDbContext(optionsBuilder.Options);
        }
    }
}