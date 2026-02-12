using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Inventory.DTOs.Warehouse;
using IMS.Application.Modules.Inventory.Interfaces;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Inventory.Services;
public class WarehouseService : IWarehouseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public WarehouseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WarehouseDto> CreateWarehouse(CreateWarehouseDto dto)
    {
        if (await _unitOfWork.Warehouses.GetOneAsync(w => w.Name == dto.Name)  is not null)
            throw new BusinessException($"Warehouse '{dto.Name}' already exists");
        if(dto.Location.Length > 200 || dto.Name.Length > 100)
            throw new BusinessException("Invalid Warehouse Name or Location");

        var createdEntity = await _unitOfWork.Warehouses.CreateAsync(_mapper.Map<Warehouse>(dto));
        await _unitOfWork.CommitAsync();
        return _mapper.Map<WarehouseDto>(createdEntity);
    }

    public async Task<WarehouseDto> UpdateWarehouse(UpdateWarehouseDto dto)
    {
       var entity = await _unitOfWork.Warehouses.GetOneAsync(w => w.Name == dto.Name);
       if (entity is null) throw new ArgumentException("Invalid Warehouse ID");
       if(dto.Location.Length > 200 || dto.Name.Length > 100)
           throw new BusinessException("Invalid Warehouse Name or Location Max 100 : 200");
       
       entity.Location = dto.Location;
       entity.Name = dto.Name;
       
       _unitOfWork.Warehouses.Update(entity);
       await _unitOfWork.CommitAsync();
        return _mapper.Map<WarehouseDto>(entity);
    }

    public async Task<WarehouseDto> GetWarehouseById(Guid id)
    {
        var entity = await _unitOfWork.Warehouses.GetOneAsync(w => w.Id == id && w.IsActive && w.IsDeleted == false);
        if (entity is null) throw new ArgumentException("Invalid Warehouse ID");
        return _mapper.Map<WarehouseDto>(entity);
    }

    public async Task<IEnumerable<WarehouseDto>> GetWarehouses()
    {
        var entities = await _unitOfWork.Warehouses.GetAsync(w => w.IsDeleted == false && w.IsActive);
        if (entities is null) throw new Exception("There is NO Warehouses.");
        return _mapper.Map<IEnumerable<WarehouseDto>>(entities);
    }

    public async Task<bool> DeleteWarehouse(Guid id)
    {
        var entity = await _unitOfWork.Warehouses.GetOneAsync(w => w.Id == id);
        if (entity is null) throw new ArgumentException("Invalid Warehouse ID");
        
        entity.IsActive = false;
        _unitOfWork.Warehouses.Update(entity);
        int result = await _unitOfWork.CommitAsync();
        return result > 0;
    }
}