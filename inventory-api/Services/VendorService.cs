using inventory_api.Dto;
using inventory_api.Interfaces;

namespace inventory_api.Services;

public class VendorService : IVendorService
{
    public Task<GetVendorResponseDto> CreateAsync(PostVendorBodyDto product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GetVendorResponseDto> CreateWithIdAsync(Guid id, PostVendorBodyDto product, CancellationToken cancellationToken)
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

    public Task<GetVendorResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GetVendorListResponseDto> GetByPageAsync(int limit, int page, string? sortBy, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ReplaceAsync(PutVendorBodyDto product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
