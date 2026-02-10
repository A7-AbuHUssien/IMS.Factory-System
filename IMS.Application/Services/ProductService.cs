using IMS.Application.DTOs.Common;
using IMS.Application.DTOs.Product;
using IMS.Application.Interfaces.Services;
using IMS.Application.Interfaces.UOW;

namespace IMS.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
}