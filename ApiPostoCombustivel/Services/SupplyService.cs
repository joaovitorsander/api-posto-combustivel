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
        private readonly SupplyRepository _supplyRepository;
        private readonly FuelRepository _fuelRepository;
        private readonly PriceRepository _priceRepository;

        public SupplyService(AppDbContext context)
        {
            _supplyRepository = new SupplyRepository(context);
            _fuelRepository = new FuelRepository(context);
            _priceRepository = new PriceRepository(context);
        }

        public IEnumerable<SupplyDTO> GetSupplies()
        {
            var supplies = _supplyRepository.GetSupplies();
            return supplies.Select(SupplyParser.ToDTO);
        }

        public SupplyDTO GetSupplyById(int id)
        {
            SupplyValidator.ValidateSupplyExistence(_supplyRepository, id);
            var supply = _supplyRepository.GetSupplyById(id);
            return SupplyParser.ToDTO(supply);
        }

        public IEnumerable<SupplyDTO> GetSuppliesByType(string fuelType)
        {
            FuelTypeValidator.ValidateType(fuelType);
            var supplies = _supplyRepository.GetSuppliesByType(fuelType);
            return supplies.Select(SupplyParser.ToDTO);
        }

        public SupplyDTO AddSupply(SupplyDTO supplyDTO)
        {
            SupplyValidator.ValidateFuelExistence(_fuelRepository, supplyDTO.FuelType);
            SupplyValidator.ValidateQuantity(supplyDTO.Quantity);

            var fuel = _fuelRepository.GetFuelByType(supplyDTO.FuelType);
            SupplyValidator.ValidateSufficientStock(fuel.Stock, supplyDTO.Quantity);

            var currentPrice = _priceRepository.GetPrices()
                .FirstOrDefault(p => p.FuelId == fuel.Id &&
                                     p.StartDate <= supplyDTO.Date &&
                                     (!p.EndDate.HasValue || p.EndDate >= supplyDTO.Date));

            if (currentPrice == null)
            {
                throw new PriceNotFoundException("Nenhum preço válido encontrado para o combustível informado na data fornecida.");
            }

            var totalValue = currentPrice.PricePerLiter * supplyDTO.Quantity;

            fuel.Stock -= supplyDTO.Quantity;
            _fuelRepository.UpdateFuel(fuel);

            var supply = SupplyParser.ToModel(supplyDTO);
            supply.TotalValue = totalValue;

            _supplyRepository.AddSupply(supply);

            return SupplyParser.ToDTO(supply);
        }


        //Só para deixar registrado que este método esta funcionando agora do jeito que eu quero, obrigado Deus
        public SupplyDTO UpdateSupply(int id, UpdateSupplyDTO updateDto)
        {
            SupplyValidator.ValidateSupplyExistence(_supplyRepository, id);
            var supply = _supplyRepository.GetSupplyById(id);

            var originalFuel = _fuelRepository.GetFuelByType(supply.FuelType);
            if (originalFuel == null)
            {
                throw new FuelNotFoundException("Combustível original não encontrado.");
            }

            bool isFuelTypeChanged = updateDto.FuelType != null && updateDto.FuelType != supply.FuelType;

            TbFuel currentFuel = originalFuel;

            if (isFuelTypeChanged)
            {
                FuelTypeValidator.ValidateType(updateDto.FuelType);
                SupplyValidator.ValidateFuelExistence(_fuelRepository, updateDto.FuelType);

                var newFuel = _fuelRepository.GetFuelByType(updateDto.FuelType);
                var requiredQuantity = updateDto.Quantity ?? supply.Quantity;
                FuelStockValidator.ValidateSufficientStock(newFuel.Stock, requiredQuantity);

                originalFuel.Stock += supply.Quantity;
                _fuelRepository.UpdateFuel(originalFuel);

                newFuel.Stock -= requiredQuantity;
                _fuelRepository.UpdateFuel(newFuel);

                currentFuel = newFuel;
                supply.FuelType = updateDto.FuelType;
            }

            if (updateDto.Quantity.HasValue)
            {
                var quantityDifference = updateDto.Quantity.Value - supply.Quantity;
                if (!isFuelTypeChanged)
                {
                    FuelStockValidator.ValidateSufficientStock(originalFuel.Stock, quantityDifference);
                    originalFuel.Stock -= quantityDifference;
                    _fuelRepository.UpdateFuel(originalFuel);
                }

                supply.Quantity = updateDto.Quantity.Value;
            }

            if (updateDto.Date.HasValue)
            {
                supply.Date = updateDto.Date.Value;
            }

            var currentPrice = _priceRepository.GetPrices()
                .FirstOrDefault(p => p.FuelId == currentFuel.Id &&
                                     p.StartDate <= supply.Date &&
                                     (!p.EndDate.HasValue || p.EndDate >= supply.Date));

            if (currentPrice == null)
            {
                throw new PriceNotFoundException("Nenhum preço válido encontrado para o combustível informado na data fornecida.");
            }

            supply.TotalValue = currentPrice.PricePerLiter * supply.Quantity;

            _supplyRepository.UpdateSupply(supply);

            return SupplyParser.ToDTO(supply);
        }

        public void DeleteSupply(int id)
        {
            SupplyValidator.ValidateSupplyExistence(_supplyRepository, id);
            _supplyRepository.DeleteSupply(id);
        }

        public object GetDailyReport(DateTime date)
        {
            var dailySupplies = _supplyRepository.GetSuppliesByDate(date);

            var totalValueByFuelType = dailySupplies
                .GroupBy(s => s.FuelType)
                .Select(g => new
                {
                    FuelType = g.Key,
                    TotalValue = g.Sum(s => s.TotalValue)
                })
                .ToList();

            var overallTotalValue = dailySupplies.Sum(s => s.TotalValue);

            var suppliedFuelTypes = dailySupplies
                .Select(s => s.FuelType)
                .Distinct()
                .ToList();

            var currentInventory = _fuelRepository.GetInventory()
                .Where(f => suppliedFuelTypes.Contains(f.Type))
                .ToList();

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
