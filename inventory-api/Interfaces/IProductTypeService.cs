using inventory_api.Dto;

namespace inventory_api.Interfaces;

public interface IProductTypeService
{
    Task<GetProductTypeListResponseDto> GetByPageAsync(int limit, int page, string? sortBy, CancellationToken cancellationToken);
    Task<GetProductTypeResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<GetProductTypeResponseDto> CreateAsync(PostProductTypeBodyDto product, CancellationToken cancellationToken);
    Task<GetProductTypeResponseDto> CreateWithIdAsync(Guid id, PostProductTypeBodyDto product, CancellationToken cancellationToken);
    Task ReplaceAsync(PutProductTypeBodyDto product, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken);
}
