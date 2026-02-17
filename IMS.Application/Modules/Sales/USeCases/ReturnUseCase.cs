using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Sales.DTOs.Order;
using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Sales.USeCases;

public class ReturnUseCase
{
    private readonly IUnitOfWork _uow;

    public ReturnUseCase(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ReturnedItemDto> Execute(CreateReturnedItemDto dto)
    {
        if (dto.Quantity <= 0)
            throw new BusinessException("Quantity must be greater than zero");

        var entity = new ReturnedItem
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            Reason = dto.Reason?.Trim(),
            Source = dto.Source,
        };

        await _uow.ReturnenItems.CreateAsync(entity);
        await _uow.CommitAsync();

        return new ReturnedItemDto
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            Quantity = entity.Quantity,
            Reason = entity.Reason,
            CreatedAt = entity.CreatedAt
        };
    }
}