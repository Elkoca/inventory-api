using AutoMapper;
using inventory_api.Data;
using inventory_api.Data.Entities;
using inventory_api.Dto;
using inventory_api.Extensions;
using inventory_api.Interfaces;
using inventory_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace inventory_api.Services;

public class ProductTypeService : IProductTypeService
{
    private readonly InventoryDbContext _dbContext;
    public readonly IMapper _mapper;

    public ProductTypeService(InventoryDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }
    public async Task<GetProductTypeListResponseDto> GetByPageAsync(int limit, int page, string? sortBy, CancellationToken cancellationToken)
    {
        string defaultSortName = "Created";
        string sortName;
        PropertyInfo sortProp;
        bool sortDesc;

        if (string.IsNullOrWhiteSpace(sortBy))
        {
            sortName = defaultSortName;
            sortDesc = false;
        }
        else
        {
            //splitting sortby (Propname.direction)
            var sortByProps = sortBy.Trim().Split(".");
            sortName = sortByProps.Count() > 0 && sortByProps.Count() < 3 ? sortByProps[0] : defaultSortName;
            sortDesc = sortByProps[1] == "desc" ? true : false;
        }
        //Her bør jeg egentlig først mappe sortName mot Dto, som igjen har en kobling mot DB Modellen

        //Get Property, If not exist, Default prop, else throw
        sortProp = typeof(ProductType).GetProperty(sortName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
            typeof(ProductType).GetProperty(defaultSortName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
            throw new Exception("Default Prop not found");


        PagedModel<ProductType> productTypes = await _dbContext.ProductTypes
            .AsNoTracking()
            .OrderByString(sortProp, sortDesc)
            .PaginateAsync(page, limit, cancellationToken);

        return _mapper.Map<GetProductTypeListResponseDto>(productTypes);
    }
    public async Task<GetProductTypeResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var productType = await _dbContext.ProductTypes
            .SingleOrDefaultAsync(x => x.ProductTypeId == id);

        if (productType == null)
            return null;

        return _mapper.Map<GetProductTypeResponseDto>(productType);
    }
    public async Task<GetProductTypeResponseDto> CreateAsync(PostProductTypeBodyDto productType, CancellationToken cancellationToken)
    {
        var newProductType = _mapper.Map<ProductType>(productType);
        _dbContext.ProductTypes.Add(newProductType);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<GetProductTypeResponseDto>(newProductType);
    }
    public async Task<GetProductTypeResponseDto> CreateWithIdAsync(Guid id, PostProductTypeBodyDto productType, CancellationToken cancellationToken)
    {
        var newProductType = _mapper.Map<ProductType>(productType);
        newProductType.ProductTypeId = id;

        _dbContext.ProductTypes.Add(newProductType);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<GetProductTypeResponseDto>(newProductType);
    }
    public async Task ReplaceAsync(PutProductTypeBodyDto productType, CancellationToken cancellationToken)
    {
        var replacedProductType = _mapper.Map<ProductType>(productType);
        _dbContext.ProductTypes.Attach(replacedProductType);
        _dbContext.ProductTypes.Update(replacedProductType);
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var productType = new ProductType { ProductTypeId = id };
        _dbContext.ProductTypes.Attach(productType);
        _dbContext.ProductTypes.Remove(productType);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.ProductTypes.AnyAsync(e => e.ProductTypeId == id);
    }
}
