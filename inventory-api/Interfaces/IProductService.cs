using inventory_api.Dto;

namespace inventory_api.Interfaces;

public interface IProductService
{
    Task<GetProductListResponseDto> GetByPageAsync(int limit, int page, string? sortBy, CancellationToken cancellationToken);
    Task<GetProductResponseDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken);
    Task<GetProductResponseDto> CreateAsync(PostProductBodyDto product, CancellationToken cancellationToken);
    Task<GetProductResponseDto> CreateWithIdAsync(Guid id, PostProductBodyDto product, CancellationToken cancellationToken);
    Task ReplaceAsync(PutProductBodyDto product, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken);
}
