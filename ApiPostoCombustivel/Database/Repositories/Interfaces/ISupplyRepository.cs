using ApiPostoCombustivel.Database.Models;
using System.Collections.Generic;

namespace ApiPostoCombustivel.Database.Repositories.Interfaces
{
    // Interface para gerenciar os registros de abastecimentos.
    // Facilita a organização e testes ao definir os métodos necessários para lidar com abastecimentos.
    public interface ISupplyRepository
    {
        // Retorna todos os abastecimentos registrados no sistema.
        // Útil para listar ou analisar todos os dados de abastecimento.
        IEnumerable<TbSupply> GetSupplies();

        // Busca um abastecimento específico pelo ID.
        // Essencial para operações que precisam de um abastecimento único.
        TbSupply GetSupplyById(int id);

        // Filtra abastecimentos pelo tipo de combustível, como "Gasolina Comum".
        // Ajuda a obter dados específicos baseados no tipo de combustível usado.
        IEnumerable<TbSupply> GetSuppliesByType(string tipoCombustivel);

        // Filtra abastecimentos por uma data específica.
        // Necessário para relatórios diários ou análises por período.
        IEnumerable<TbSupply> GetSuppliesByDate(DateTime data);

        // Adiciona um novo registro de abastecimento.
        // Usado para inserir novos dados no sistema.
        void AddSupply(TbSupply abastecimento);

        // Atualiza informações de um abastecimento existente.
        // Necessário para corrigir ou modificar dados de registros já feitos.
        void UpdateSupply(TbSupply abastecimento);

        // Remove um registro de abastecimento pelo ID.
        // Útil para exclusão de dados que não são mais relevantes ou foram registrados incorretamente.
        void DeleteSupply(int id);
    }
}
