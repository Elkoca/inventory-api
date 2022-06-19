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

public class VendorService : IVendorService
{
    private readonly InventoryDbContext _dbContext;
    public readonly IMapper _mapper;

    public VendorService(InventoryDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<GetVendorListResponseDto> GetByPageAsync(int limit, int page, string? sortBy, CancellationToken cancellationToken)
    {
        string defaultSortName = "Name";
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
        sortProp = typeof(Vendor).GetProperty(sortName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
            typeof(Vendor).GetProperty(defaultSortName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
            throw new Exception("Default Prop not found");


        PagedModel<Vendor> vendors = await _dbContext.Vendors
            .AsNoTracking()
            .OrderByString(sortProp, sortDesc)
            .PaginateAsync(page, limit, cancellationToken);

        return _mapper.Map<GetVendorListResponseDto>(vendors);
    }
    public async Task<GetVendorResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var vendors = await _dbContext.Vendors
            .SingleOrDefaultAsync(x => x.VendorId == id);

        if (vendors == null)
            return null;

        return _mapper.Map<GetVendorResponseDto>(vendors);
    }
    public async Task<GetVendorResponseDto> CreateAsync(PostVendorBodyDto vendor, CancellationToken cancellationToken)
    {
        var newVendor = _mapper.Map<Vendor>(vendor);
        _dbContext.Vendors.Add(newVendor);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<GetVendorResponseDto>(newVendor);
    }
    public async Task<GetVendorResponseDto> CreateWithIdAsync(Guid id, PostVendorBodyDto vendor, CancellationToken cancellationToken)
    {
        var newVendor = _mapper.Map<Vendor>(vendor);
        newVendor.VendorId = id;

        _dbContext.Vendors.Add(newVendor);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<GetVendorResponseDto>(newVendor);
    }
    public async Task ReplaceAsync(PutVendorBodyDto vendor, CancellationToken cancellationToken)
    {
        var replacedVendor = _mapper.Map<Vendor>(vendor);
        _dbContext.Vendors.Attach(replacedVendor);
        _dbContext.Vendors.Update(replacedVendor);
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        _dbContext.Vendors.Remove(
            await _dbContext.Vendors
                .Include(x => x.Products)
                .SingleAsync(x => x.VendorId == id)
            );
        await _dbContext.SaveChangesAsync();
    }
    public async Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Vendors.AnyAsync(e => e.VendorId == id);
    }
}
