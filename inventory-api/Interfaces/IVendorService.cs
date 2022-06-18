using inventory_api.Dto;

namespace inventory_api.Interfaces;

public interface IVendorService
{
    Task<GetVendorListResponseDto> GetByPageAsync(int limit, int page, string? sortBy, CancellationToken cancellationToken);
    Task<GetVendorResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<GetVendorResponseDto> CreateAsync(PostVendorBodyDto product, CancellationToken cancellationToken);
    Task<GetVendorResponseDto> CreateWithIdAsync(Guid id, PostVendorBodyDto product, CancellationToken cancellationToken);
    Task ReplaceAsync(PutVendorBodyDto product, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken);
}
