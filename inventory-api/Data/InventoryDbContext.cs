using inventory_api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace inventory_api.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<ProductType> ProductType { get; set; }
    public DbSet<Vendor> Vendor { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Products
        modelBuilder.Entity<Product>()
            .HasKey(b => b.ProductId);
        modelBuilder.Entity<Product>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


        modelBuilder.Entity<Product>()
            .Property(b => b.LastUpdated)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnUpdate()
            .IsRequired();
            //.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


        modelBuilder.Entity<Product>()
            .Property(b => b.Name)
            .IsRequired();
        modelBuilder.Entity<Product>()
            .Property(b => b.Title)
            .IsRequired();

        //Mapping

        modelBuilder.Entity<Product>()
            .HasOne(b => b.Currency)
            .WithOne(p => p.Product)
            .HasForeignKey<Currency>(p => p.ProductId);

        modelBuilder.Entity<Product>()
            .HasOne(b => b.ProductType)
            .WithMany(b => b.Products)
            .HasForeignKey(b => b.ProductTypeId);

        modelBuilder.Entity<Product>()
            .HasOne(b => b.Vendor)
            .WithMany(b => b.Products)
            .HasForeignKey(b => b.VendorId);




        //Currency 
        modelBuilder.Entity<Currency>()
            .HasKey(b => b.CurrencyId);



        //ProductType 
        modelBuilder.Entity<ProductType>()
            .HasKey(b => b.ProductTypeId);


        //Vendor 
        modelBuilder.Entity<Vendor>()
            .HasKey(b => b.VendorId);
    }
    //public override int SaveChanges(bool acceptAllChangesOnSuccess)
    //{
    //    beforeSaving();
    //    return base.SaveChanges(acceptAllChangesOnSuccess);
    //}

    //public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    //{
    //    beforeSaving();
    //    return (await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
    //}

    //private void beforeSaving()
    //{
    //    DateTime currentTime = DateTime.UtcNow;

    //    this.ChangeTracker.DetectChanges();
    //    var a = this.ChangeTracker.Entries<Product>();
    //    var myChangedEntities = this.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
    //    foreach (var changedEntity in myChangedEntities)
    //    {
    //        if (changedEntity.Entity is IEntity entity)
    //        {
    //            switch (changedEntity.State)
    //            {

    //                case EntityState.Added:
    //                    {
    //                        //entity.Created = currentTime;
    //                        //entity.LastUpdated = currentTime;
    //                        break;
    //                    }
    //                case EntityState.Modified:
    //                    {
    //                        //Entry(entity).Reference(x => x.Created).IsModified = false; // Denne fikses med:  .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    //                        entity.LastUpdated = currentTime; //Denne fungerer ikke uten med ValueGeneratedOnAddOrUpdate
    //                        break;
    //                    }
    //            }
    //        }
    //    }
    //}
}