using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.DTO.CombustivelDTO;
using ApiPostoCombustivel.Parser;
using ApiPostoCombustivel.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Services
{
    public class FuelService
    {
        private readonly FuelRepository _fuelRepository;

        public FuelService(AppDbContext context)
        {
            _fuelRepository = new FuelRepository(context);
        }

        public IEnumerable<FuelDTO> GetInventory()
        {
            var fuels = _fuelRepository.GetInventory();
            return fuels.Select(FuelParser.ToDTO);
        }

        public FuelDTO GetFuelById(int id)
        {
            FuelExistenceValidator.ValidateFuelExistence(_fuelRepository, id);

            var fuel = _fuelRepository.GetFuelById(id);
            return FuelParser.ToDTO(fuel);
        }

        public FuelDTO GetFuelByType(string tipo)
        {
            FuelTypeValidator.ValidateType(tipo);

            var fuel = _fuelRepository.GetFuelByType(tipo);
            return fuel != null ? FuelParser.ToDTO(fuel) : null;
        }

        public FuelDTO AddFuel(FuelDTO fuelDto)
        {
            FuelTypeValidator.ValidateType(fuelDto.Type);
            FuelStockValidator.ValidateStock(fuelDto.Stock);
            FuelExistenceValidator.ValidateFuelAlreadyRegistered(_fuelRepository, fuelDto.Type);

            var fuel = FuelParser.ToModel(fuelDto);
            _fuelRepository.AddFuel(fuel);
            return FuelParser.ToDTO(fuel);
        }

        public void UpdateFuel(int id, UpdateFuelDTO updateDto)
        {
            FuelExistenceValidator.ValidateFuelExistence(_fuelRepository, id);

            var fuel = _fuelRepository.GetFuelById(id);

            if (updateDto.Type != null)
            {
                FuelTypeValidator.ValidateType(updateDto.Type);
                fuel.Type = updateDto.Type;
            }

            if (updateDto.Stock.HasValue)
            {
                FuelStockValidator.ValidateStock(updateDto.Stock);
                fuel.Stock = updateDto.Stock.Value;
            }

            _fuelRepository.UpdateFuel(fuel);
        }

        public void DeleteFuel(int id)
        {
            FuelExistenceValidator.ValidateFuelExistence(_fuelRepository, id);

            _fuelRepository.DeleteFuel(id);
        }
    }
}
