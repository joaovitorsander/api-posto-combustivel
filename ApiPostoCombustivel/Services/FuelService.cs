using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.DTO.CombustivelDTO;
using ApiPostoCombustivel.Exceptions;
using ApiPostoCombustivel.Parser;
using ApiPostoCombustivel.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Services
{
    public class FuelService
    {
        // Repositório de combustível responsável pela interação com o banco de dados.
        private readonly FuelRepository _fuelRepository;

        // Construtor que inicializa o repositório de combustível utilizando o contexto do banco de dados.
        public FuelService(AppDbContext context)
        {
            _fuelRepository = new FuelRepository(context);
        }

        // Retorna a lista de combustíveis disponíveis no inventário.
        // Realiza a conversão dos modelos de banco de dados (TbFuel) para DTOs (FuelDTO).
        public IEnumerable<FuelDTO> GetInventory()
        {
            var fuels = _fuelRepository.GetInventory(); // Obtém todos os combustíveis do repositório.
            return fuels.Select(FuelParser.ToDTO); // Converte cada combustível para o formato DTO.
        }

        // Obtém os detalhes de um combustível específico com base no ID fornecido.
        // Valida se o combustível existe antes de buscar no repositório.
        public FuelDTO GetFuelById(int id)
        {
            // Valida se o combustível com o ID fornecido existe no banco de dados.
            FuelExistenceValidator.ValidateFuelExistence(_fuelRepository, id);

            // Recupera o combustível do banco de dados e o converte para DTO.
            var fuel = _fuelRepository.GetFuelById(id);
            return FuelParser.ToDTO(fuel);
        }

        // Obtém os detalhes de um combustível específico com base no tipo fornecido.
        // Valida o tipo e verifica se o combustível está registrado.
        public FuelDTO GetFuelByType(string tipo)
        {
            // Valida o tipo de combustível fornecido.
            FuelTypeValidator.ValidateType(tipo);

            // Recupera o combustível do banco de dados baseado no tipo.
            var fuel = _fuelRepository.GetFuelByType(tipo);

            // Lança uma exceção caso o combustível não seja encontrado.
            if (fuel == null)
            {
                throw new FuelNotFoundException($"Combustível do tipo '{tipo}' não encontrado.");
            }

            // Converte o combustível para DTO e retorna.
            return FuelParser.ToDTO(fuel);
        }

        // Adiciona um novo combustível ao inventário.
        // Realiza diversas validações antes de adicionar o combustível ao banco de dados.
        public FuelDTO AddFuel(FuelDTO fuelDto)
        {
            // Valida o tipo do combustível.
            FuelTypeValidator.ValidateType(fuelDto.Type);

            // Valida o estoque inicial fornecido.
            FuelStockValidator.ValidateStock(fuelDto.Stock);

            // Verifica se o combustível já está registrado no sistema.
            FuelExistenceValidator.ValidateFuelAlreadyRegistered(_fuelRepository, fuelDto.Type);

            // Converte o DTO para o modelo do banco e adiciona ao repositório.
            var fuel = FuelParser.ToModel(fuelDto);
            _fuelRepository.AddFuel(fuel);

            // Converte o modelo salvo de volta para DTO e retorna.
            return FuelParser.ToDTO(fuel);
        }

        // Atualiza os dados de um combustível existente com base no ID fornecido.
        // Permite atualização de tipo e/ou estoque com validações apropriadas.
        public void UpdateFuel(int id, UpdateFuelDTO updateDto)
        {
            // Valida se o combustível com o ID fornecido existe.
            FuelExistenceValidator.ValidateFuelExistence(_fuelRepository, id);

            // Obtém o combustível do banco de dados.
            var fuel = _fuelRepository.GetFuelById(id);

            // Atualiza o tipo de combustível, se fornecido.
            if (updateDto.Type != null)
            {
                FuelTypeValidator.ValidateType(updateDto.Type);
                fuel.Type = updateDto.Type;
            }

            // Atualiza o estoque de combustível, se fornecido.
            if (updateDto.Stock.HasValue)
            {
                FuelStockValidator.ValidateStock(updateDto.Stock);
                fuel.Stock = updateDto.Stock.Value;
            }

            // Salva as alterações no banco de dados.
            _fuelRepository.UpdateFuel(fuel);
        }

        // Remove um combustível do inventário com base no ID fornecido.
        // Valida se o combustível existe antes de excluí-lo.
        public void DeleteFuel(int id)
        {
            // Valida se o combustível com o ID fornecido existe.
            FuelExistenceValidator.ValidateFuelExistence(_fuelRepository, id);

            // Exclui o combustível do banco de dados.
            _fuelRepository.DeleteFuel(id);
        }
    }
}
