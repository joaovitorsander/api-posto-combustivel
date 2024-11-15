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

        public virtual DbSet<TbSupply> Supplies { get; set; }
        public virtual DbSet<TbFuel> Fuels { get; set; } 
        public virtual DbSet<TbPrice> Prices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("DBPosto");
            }
        }
    }
}
