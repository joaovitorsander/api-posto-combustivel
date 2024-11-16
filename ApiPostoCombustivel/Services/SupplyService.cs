using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Repositories.Interfaces;
using ApiPostoCombustivel.DTO.AbastecimentoDTO;
using ApiPostoCombustivel.Parser;
using System.Collections.Generic;
using System.Linq;
using ApiPostoCombustivel.Validations;
using ApiPostoCombustivel.Exceptions;
using ApiPostoCombustivel.Database.Models;

namespace ApiPostoCombustivel.Services
{
    public class SupplyService
    {
        // Repositórios necessários para interagir com os dados de abastecimento, combustíveis e preços no banco de dados.
        private readonly SupplyRepository _supplyRepository;
        private readonly FuelRepository _fuelRepository;
        private readonly PriceRepository _priceRepository;

        // Construtor que inicializa os repositórios de abastecimento, combustível e preço, usando o contexto do banco de dados.
        public SupplyService(AppDbContext context)
        {
            _supplyRepository = new SupplyRepository(context);
            _fuelRepository = new FuelRepository(context);
            _priceRepository = new PriceRepository(context);
        }

        // Retorna uma lista de abastecimentos, convertendo os modelos de banco de dados para DTOs.
        public IEnumerable<SupplyDTO> GetSupplies()
        {
            var supplies = _supplyRepository.GetSupplies(); // Obtém todos os abastecimentos do repositório.
            return supplies.Select(SupplyParser.ToDTO); // Converte os modelos para DTOs e retorna.
        }

        // Retorna um abastecimento específico pelo ID, validando sua existência antes de buscar.
        public SupplyDTO GetSupplyById(int id)
        {
            // Valida se o abastecimento com o ID fornecido existe no banco de dados.
            SupplyValidator.ValidateSupplyExistence(_supplyRepository, id);

            // Obtém o abastecimento do banco de dados e converte para DTO.
            var supply = _supplyRepository.GetSupplyById(id);
            return SupplyParser.ToDTO(supply);
        }

        // Retorna os abastecimentos de um tipo específico de combustível.
        public IEnumerable<SupplyDTO> GetSuppliesByType(string fuelType)
        {
            // Valida se o tipo de combustível fornecido é válido.
            FuelTypeValidator.ValidateType(fuelType);

            // Obtém os abastecimentos do repositório de acordo com o tipo de combustível.
            var supplies = _supplyRepository.GetSuppliesByType(fuelType);
            return supplies.Select(SupplyParser.ToDTO); // Converte os modelos para DTOs e retorna.
        }

        // Adiciona um novo abastecimento após realizar várias validações.
        public SupplyDTO AddSupply(SupplyDTO supplyDTO)
        {
            // Valida se o combustível existe no banco de dados.
            SupplyValidator.ValidateFuelExistence(_fuelRepository, supplyDTO.FuelType);

            // Valida se a quantidade de combustível no abastecimento é válida.
            SupplyValidator.ValidateQuantity(supplyDTO.Quantity);

            // Obtém o combustível correspondente ao tipo fornecido.
            var fuel = _fuelRepository.GetFuelByType(supplyDTO.FuelType);

            // Valida se há estoque suficiente de combustível para o abastecimento.
            SupplyValidator.ValidateSufficientStock(fuel.Stock, supplyDTO.Quantity);

            // Obtém o preço atual do combustível para a data do abastecimento.
            var currentPrice = _priceRepository.GetPrices()
                .FirstOrDefault(p => p.FuelId == fuel.Id &&
                                     p.StartDate <= supplyDTO.Date &&
                                     (!p.EndDate.HasValue || p.EndDate >= supplyDTO.Date));

            // Se não houver preço válido para a data, lança uma exceção.
            if (currentPrice == null)
            {
                throw new PriceNotFoundException("Nenhum preço válido encontrado para o combustível informado na data fornecida.");
            }

            // Calcula o valor total do abastecimento (preço por litro * quantidade).
            var totalValue = currentPrice.PricePerLiter * supplyDTO.Quantity;

            // Atualiza o estoque do combustível.
            fuel.Stock -= supplyDTO.Quantity;
            _fuelRepository.UpdateFuel(fuel);

            // Converte o DTO para o modelo de abastecimento e adiciona o valor total.
            var supply = SupplyParser.ToModel(supplyDTO);
            supply.TotalValue = totalValue;

            // Adiciona o abastecimento ao banco de dados.
            _supplyRepository.AddSupply(supply);

            // Converte o modelo salvo de volta para DTO e retorna.
            return SupplyParser.ToDTO(supply);
        }

        // Atualiza um abastecimento existente, considerando mudanças no tipo de combustível e quantidade.
        public SupplyDTO UpdateSupply(int id, UpdateSupplyDTO updateDto)
        {
            // Valida se o abastecimento com o ID fornecido existe no banco de dados.
            SupplyValidator.ValidateSupplyExistence(_supplyRepository, id);
            var supply = _supplyRepository.GetSupplyById(id);

            // Obtém o combustível original utilizado no abastecimento.
            var originalFuel = _fuelRepository.GetFuelByType(supply.FuelType);
            if (originalFuel == null)
            {
                throw new FuelNotFoundException("Combustível original não encontrado.");
            }

            bool isFuelTypeChanged = updateDto.FuelType != null && updateDto.FuelType != supply.FuelType;

            TbFuel currentFuel = originalFuel;

            // Se o tipo de combustível foi alterado, valida e atualiza os combustíveis envolvidos.
            if (isFuelTypeChanged)
            {
                FuelTypeValidator.ValidateType(updateDto.FuelType); // Valida o novo tipo de combustível.
                SupplyValidator.ValidateFuelExistence(_fuelRepository, updateDto.FuelType); // Valida a existência do novo combustível.

                var newFuel = _fuelRepository.GetFuelByType(updateDto.FuelType);
                var requiredQuantity = updateDto.Quantity ?? supply.Quantity;
                FuelStockValidator.ValidateSufficientStock(newFuel.Stock, requiredQuantity); // Verifica se há estoque suficiente do novo combustível.

                // Atualiza o estoque do combustível original.
                originalFuel.Stock += supply.Quantity;
                _fuelRepository.UpdateFuel(originalFuel);

                // Atualiza o estoque do novo combustível.
                newFuel.Stock -= requiredQuantity;
                _fuelRepository.UpdateFuel(newFuel);

                currentFuel = newFuel;
                supply.FuelType = updateDto.FuelType;
            }

            // Se a quantidade foi alterada, atualiza o estoque do combustível.
            if (updateDto.Quantity.HasValue)
            {
                var quantityDifference = updateDto.Quantity.Value - supply.Quantity;
                if (!isFuelTypeChanged)
                {
                    FuelStockValidator.ValidateSufficientStock(originalFuel.Stock, quantityDifference); // Verifica se o estoque do combustível original é suficiente.
                    originalFuel.Stock -= quantityDifference;
                    _fuelRepository.UpdateFuel(originalFuel);
                }

                supply.Quantity = updateDto.Quantity.Value;
            }

            // Atualiza a data do abastecimento, se fornecida.
            if (updateDto.Date.HasValue)
            {
                supply.Date = updateDto.Date.Value;
            }

            // Obtém o preço atual do combustível para a data do abastecimento atualizado.
            var currentPrice = _priceRepository.GetPrices()
                .FirstOrDefault(p => p.FuelId == currentFuel.Id &&
                                     p.StartDate <= supply.Date &&
                                     (!p.EndDate.HasValue || p.EndDate >= supply.Date));

            // Se não houver preço válido para a data, lança uma exceção.
            if (currentPrice == null)
            {
                throw new PriceNotFoundException("Nenhum preço válido encontrado para o combustível informado na data fornecida.");
            }

            // Atualiza o valor total do abastecimento.
            supply.TotalValue = currentPrice.PricePerLiter * supply.Quantity;

            // Atualiza o abastecimento no banco de dados.
            _supplyRepository.UpdateSupply(supply);

            // Converte o modelo atualizado de volta para DTO e retorna.
            return SupplyParser.ToDTO(supply);
        }

        // Exclui um abastecimento pelo ID, validando sua existência antes.
        public void DeleteSupply(int id)
        {
            // Valida se o abastecimento com o ID fornecido existe no banco de dados.
            SupplyValidator.ValidateSupplyExistence(_supplyRepository, id);

            // Exclui o abastecimento do banco de dados.
            _supplyRepository.DeleteSupply(id);
        }

        // Gera um relatório diário de abastecimentos, incluindo valores totais por tipo de combustível e estoque atual.
        public object GetDailyReport(DateTime date)
        {
            // Obtém todos os abastecimentos realizados na data fornecida.
            var dailySupplies = _supplyRepository.GetSuppliesByDate(date);

            // Agrupa os abastecimentos por tipo de combustível e calcula o valor total por tipo.
            var totalValueByFuelType = dailySupplies
                .GroupBy(s => s.FuelType)
                .Select(g => new
                {
                    FuelType = g.Key,
                    TotalValue = g.Sum(s => s.TotalValue)
                })
                .ToList();

            // Calcula o valor total geral dos abastecimentos no dia.
            var overallTotalValue = dailySupplies.Sum(s => s.TotalValue);

            // Obtém os tipos de combustíveis abastecidos no dia.
            var suppliedFuelTypes = dailySupplies
                .Select(s => s.FuelType)
                .Distinct()
                .ToList();

            // Obtém o inventário atual dos combustíveis abastecidos no dia.
            var currentInventory = _fuelRepository.GetInventory()
                .Where(f => suppliedFuelTypes.Contains(f.Type))
                .ToList();

            // Retorna um objeto com o relatório diário, incluindo abastecimentos, inventário e valores totais.
            return new
            {
                DailySupplies = dailySupplies,
                CurrentInventory = currentInventory,
                TotalValueByFuelType = totalValueByFuelType,
                OverallTotalValue = overallTotalValue
            };
        }
    }
}
