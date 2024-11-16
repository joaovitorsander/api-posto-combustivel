using ApiPostoCombustivel.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPostoCombustivel.Database
{
    // Classe que representa o contexto do banco de dados para a aplicação.
    // Essa classe gerencia a conexão com o banco de dados e as entidades mapeadas.
    public class AppDbContext : DbContext
    {
        // O construtor recebe as opções de configuração do contexto.
        // Isso permite configurar o banco de dados ao inicializar o contexto.
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Representa a tabela de abastecimentos no banco de dados.
        // Essa propriedade permite realizar operações como consultas, adições e remoções de registros de abastecimento.
        public virtual DbSet<TbSupply> Supplies { get; set; }

        // Representa a tabela de combustíveis no banco de dados.
        // Permite trabalhar com os dados relacionados a combustíveis, como estoque e tipos disponíveis.
        public virtual DbSet<TbFuel> Fuels { get; set; }

        // Representa a tabela de preços no banco de dados.
        // Armazena informações sobre os preços de cada tipo de combustível.
        public virtual DbSet<TbPrice> Prices { get; set; }

        // Método que configura o banco de dados caso não esteja configurado.
        // Aqui, estamos usando um banco de dados em memória para testes ou desenvolvimento.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("DBPosto");
            }
        }
    }
}
