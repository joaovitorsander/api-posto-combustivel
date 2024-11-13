using ApiPostoCombustivel.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPostoCombustivel.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbAbastecimento> Abastecimentos { get; set; }
        public virtual DbSet<TbCombustivel> Combustiveis { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("DBPosto");
            }
        }
    }
}
