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

        public virtual DbSet<TbAbastecimento> Abastecimentos { get; set; } // Renomeado corretamente
        public virtual DbSet<TbCombustivel> Combustiveis { get; set; } // Renomeado corretamente

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // Garantindo que o banco de dados em memória seja usado apenas quando necessário
            {
                optionsBuilder.UseInMemoryDatabase("DBPosto");
            }
        }
    }
}
