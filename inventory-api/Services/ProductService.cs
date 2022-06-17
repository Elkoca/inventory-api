using inventory_api.Data;
using inventory_api.Data.Entities;
using inventory_api.Dto;
using inventory_api.Extensions;
using inventory_api.Interfaces;
using inventory_api.Models;
using Microsoft.EntityFrameworkCore;

namespace inventory_api.Services;

public class ProductService : IProductService
{
    private readonly InventoryDbContext _dbContext;

    public ProductService(InventoryDbContext context)
    {
        _dbContext = context;
    }

    public async Task<GetProductListResponseDto> GetByPageAsync(int limit, int page, CancellationToken cancellationToken)
    {
        PagedModel<Product> products = await _dbContext.Products
                       .AsNoTracking()
                       .OrderBy(p => p.CreatedAt)
                       .PaginateAsync(page, limit, cancellationToken);

        return GenerateProductListResponse(products);
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


    public GetProductListResponseDto GenerateProductListResponse(PagedModel<Product> pagedProducts)
    {
        return new GetProductListResponseDto
        {
            CurrentPage = pagedProducts.CurrentPage,
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
