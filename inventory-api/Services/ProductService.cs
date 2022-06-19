using AutoMapper;
using inventory_api.Data;
using inventory_api.Data.Entities;
using inventory_api.Dto;
using inventory_api.Extensions;
using inventory_api.Interfaces;
using inventory_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Expressions;

namespace inventory_api.Services;

public class ProductService : IProductService
{
    private readonly InventoryDbContext _dbContext;
    public readonly IMapper _mapper;

    public ProductService(InventoryDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<GetProductListResponseDto> GetByPageAsync(int limit, int page, string? sortBy, string? searchString, CancellationToken cancellationToken)
    {
        string defaultSortName = "Created";
        string sortName;
        PropertyInfo sortProp;
        bool sortDesc;
        PagedModel<Product> products;

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
        sortProp = typeof(Product).GetProperty(sortName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
            typeof(Product).GetProperty(defaultSortName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? 
            throw new Exception("Default Prop not found");


        if(searchString == null)
        {
            products = await _dbContext.Products
                .AsNoTracking()
                .OrderByString(sortProp, sortDesc)
                .Include(x => x.Price)
                .PaginateAsync(page, limit, cancellationToken);
        }
        else
        {
            searchString = searchString.Trim();
            products = await _dbContext.Products
                .AsNoTracking()
                .OrderByString(sortProp, sortDesc)
                .Include(x => x.Price)
                //.Where(x => x.Name )
                .Where(x => 
                    (x.Name != null && x.Name.Contains(searchString)) || //
                    //(x.Title != null && x.Title.Contains(searchString)) || //Name
                    (x.ArticleNo != null && x.ArticleNo.ToString().Contains(searchString))) //Partial
                .PaginateAsync(page, limit, cancellationToken);
        }

        
        return _mapper.Map<GetProductListResponseDto>(products);
    }

    public async Task<GetProductResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var productContext = _dbContext.Products
            .Include(x => x.Price)
            .Include(x => x.ProductType)
            .Include(x => x.Vendor);

        var products = await productContext.SingleOrDefaultAsync(x => x.ProductId == id);

        if (products == null)
            return null;

        return _mapper.Map<GetProductResponseDto>(products);
    }
    public async Task<GetProductResponseDto> CreateAsync(PostProductBodyDto product, CancellationToken cancellationToken)
    {
        //Automapper
        var newProduct = _mapper.Map<Product>(product);

        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();

        //Automapper
        return _mapper.Map<GetProductResponseDto>(newProduct);
    }

    public async Task<GetProductResponseDto> CreateWithIdAsync(Guid id, PostProductBodyDto product, CancellationToken cancellationToken)
    {
        var newProduct = _mapper.Map<Product>(product);
        newProduct.ProductId = id;

        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();

        //Automapper
        return _mapper.Map<GetProductResponseDto>(newProduct);
    }
    public async Task ReplaceAsync(PutProductBodyDto product, CancellationToken cancellationToken)
    {
        //Må se mer på
        //Automapper

        var replacedProduct = _mapper.Map<Product>(product);
        var oldPrice = await _dbContext.Prices.SingleOrDefaultAsync(x => x.ProductId == replacedProduct.ProductId, cancellationToken: cancellationToken);

        if(replacedProduct.Price != null && oldPrice != null)
            replacedProduct.Price.ProductId = oldPrice.PriceId;

        _dbContext.Products.Attach(replacedProduct);
        _dbContext.Products.Update(replacedProduct);
        //_dbContext.Entry(replacedProduct).Reference(x => x.Price.ProductId).IsModified = false;
        //_dbContext.Entry(replacedProduct).Property(x => x.Price.ProductId).IsModified = false;
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        _dbContext.Products.Remove(
           await _dbContext.Products
                .Include(x => x.Price)
                .SingleAsync(x => x.ProductId == id)
            );
        await _dbContext.SaveChangesAsync();
    }
    public async Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Products.AnyAsync(e => e.ProductId == id);
    }
}
