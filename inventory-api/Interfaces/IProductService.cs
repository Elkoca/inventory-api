using inventory_api.Dto;

namespace inventory_api.Interfaces;

public interface IProductService
{
    Task<GetProductListResponseDto> GetByPageAsync(int limit, int page, CancellationToken cancellationToken);
}
