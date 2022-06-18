﻿using inventory_api.Data;
using inventory_api.Data.Entities;
using inventory_api.Dto;
using inventory_api.Extensions;
using inventory_api.Interfaces;
using inventory_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace inventory_api.Services;

public class ProductService : IProductService
{
    private readonly InventoryDbContext _dbContext;

    public ProductService(InventoryDbContext context)
    {
        _dbContext = context;
    }

    public async Task<GetProductListResponseDto> GetByPageAsync(int limit, int page, string? sortBy, CancellationToken cancellationToken)
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
        sortProp = typeof(Product).GetProperty(sortName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
            typeof(Product).GetProperty(defaultSortName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? 
            throw new Exception("Default Prop not found");


        PagedModel<Product> products = await _dbContext.Products
            .AsNoTracking()
            .OrderByString(sortProp, sortDesc)
            .PaginateAsync(page, limit, cancellationToken);

        return GenerateProductListResponse(products);
    }

    public async Task<GetProductResponseDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products
            .FindAsync(guid);

        if (products == null)
            return null;

        return new GetProductResponseDto()
        {
            Id = products.Id,
            Description = products.Description,
            Name = products.Name,
            Title = products.Title
        };
    }
    public async Task<GetProductResponseDto> CreateAsync(PostProductBodyDto product, CancellationToken cancellationToken)
    {
        //Automapper
        var newProduct = new Product
        {
            Name = product.Name,
            Title = product.Title,
            Description = product.Description
        };
        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();

        //Automapper
        return new GetProductResponseDto()
        {
            Id = newProduct.Id,
            Description = newProduct.Description,
            Name = newProduct.Name,
            Title = newProduct.Title
        };
    }

    public async Task<GetProductResponseDto> CreateWithIdAsync(Guid id, PostProductBodyDto product, CancellationToken cancellationToken)
    {
        var newProduct = new Product
        {
            Id = (Guid)id,
            Name = product.Name,
            Title = product.Title,
            Description = product.Description
        };
        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();

        //Automapper
        return new GetProductResponseDto()
        {
            Id = newProduct.Id,
            Description = newProduct.Description,
            Name = newProduct.Name,
            Title = newProduct.Title
        };
    }
    public async Task ReplaceAsync(PutProductBodyDto product, CancellationToken cancellationToken)
    {
        //Automapper
        var replacedProduct = new Product
        {
            Id = product.Id,
            Name = product.Name,
            Title = product.Title,
            Description = product.Description,
        };

        _dbContext.Products.Attach(replacedProduct);
        _dbContext.Products.Update(replacedProduct);
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = new Product { Id = id };
        _dbContext.Products.Attach(product);
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Products.AnyAsync(e => e.Id == id);
    }

    //Eksempel på hvordan jeg ser for meg at sorting blir etterhvert
    //public async Task<GetProductListResponseDto> GetByPageAsync(int limit, int page, string sortOrder, CancellationToken cancellationToken)
    //{
    //    PagedModel<Product> products = await _dbContext.Products
    //                   .AsNoTracking()
    //                   .OrderBy(p => p.CreatedAt)
    //                   .PaginateAsync(page, limit, cancellationToken);

    //    return GenerateProductListResponse(products);
    //}

    //AUTOMAPPER!
    public GetProductListResponseDto GenerateProductListResponse(PagedModel<Product> pagedProducts)
    {
        return new GetProductListResponseDto
        {
            CurrentPage = pagedProducts.CurrentPage,
            PageSize = pagedProducts.PageSize,
            TotalPages = pagedProducts.TotalPages,
            TotalItems = pagedProducts.TotalItems,
            Items = pagedProducts.Items.Select(p => new GetProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Title = p.Title,
                Description = p.Description
            }).ToList()
        };
    }
}
