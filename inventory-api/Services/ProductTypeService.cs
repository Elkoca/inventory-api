using inventory_api.Dto;
using inventory_api.Interfaces;

namespace inventory_api.Services;

public class ProductTypeService : IProductTypeService
{
    public Task<GetProductTypeResponseDto> CreateAsync(PostProductTypeBodyDto product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GetProductTypeResponseDto> CreateWithIdAsync(Guid id, PostProductTypeBodyDto product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GetProductTypeResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GetProductTypeListResponseDto> GetByPageAsync(int limit, int page, string? sortBy, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ReplaceAsync(PutProductTypeBodyDto product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
