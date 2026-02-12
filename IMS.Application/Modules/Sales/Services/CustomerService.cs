using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.DTOs;
using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Sales.DTOs.Customer;
using IMS.Application.Modules.Sales.Filters;
using IMS.Application.Modules.Sales.Interfaces;
using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Sales.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerDto> GetCustomerByIdAsync(Guid id)
    {
        Customer? customer = await _unitOfWork.Customers.GetOneAsync(c => c.Id == id);
        if (customer == null) throw new ArgumentException($"Customer with id: {id} is not found");
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<PaginatedApiResponse<CustomerDto>?> GetAllCustomersAsync(CustomerFilter filter)
    {
        IQueryable<Customer>? query = _unitOfWork.Customers.Query();
        if (filter.Search != null)
        {
            filter.Search = filter.Search.Trim().ToLower();
            query = query.Where(c =>
                c.Name.ToLower().Contains(filter.Search) ||
                c.Email.ToLower().Contains(filter.Search) ||
                c.Phone.ToLower().Contains(filter.Search) ||
                c.Address.ToLower().Contains(filter.Search)
            );
        }

        var totalCount = query?.Count() ?? 0;

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return new PaginatedApiResponse<CustomerDto>(items, filter.PageNumber, filter.PageSize, totalCount,
            "Customers fetched");
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto customerDto)
    {
        if(customerDto == null) throw new ArgumentException("CustomerDto is null");
        if (String.IsNullOrWhiteSpace(customerDto.Name) || 
            String.IsNullOrWhiteSpace(customerDto.Email) || 
            String.IsNullOrWhiteSpace(customerDto.Phone) || 
            String.IsNullOrWhiteSpace(customerDto.Address))
            throw new ArgumentException("All fields are required");

        Customer? entity = _mapper.Map<Customer>(customerDto);
        entity.IsDeleted = false;   
        await _unitOfWork.Customers.CreateAsync(entity);
        await _unitOfWork.CommitAsync();
       
        return _mapper.Map<CustomerDto>(entity);
    }
    
    public async Task<CustomerDto?> UpdateCustomerAsync(Guid id, UpdateCustomerDto customerDto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteCustomerAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}