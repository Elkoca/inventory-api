using inventory_api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace inventory_api.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Vendor> Vendors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Key Mapping

        modelBuilder.Entity<Product>()
            .HasOne(b => b.Price)
            .WithOne(p => p.Product)
            .HasForeignKey<Price>(p => p.ProductId);
        modelBuilder.Entity<Product>()
            .HasOne(b => b.ProductType)
            .WithMany(b => b.Products)
            .HasForeignKey(b => b.ProductTypeId);
        modelBuilder.Entity<Product>()
            .HasOne(b => b.Vendor)
            .WithMany(b => b.Products)
            .HasForeignKey(b => b.VendorId);

        //Products

        modelBuilder.Entity<Product>()
            .HasKey(b => b.ProductId);
        modelBuilder.Entity<Product>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnAdd()
            .IsRequired(true)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<Product>()
            .Property(b => b.LastUpdated)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnUpdate()
            .IsRequired(true);
            //.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<Product>()
            .Property(b => b.Name)
            .IsRequired(true);
        modelBuilder.Entity<Product>()
            .Property(b => b.Description)
            .IsRequired(false);
        modelBuilder.Entity<Product>()
            .Property(b => b.Stock);
        modelBuilder.Entity<Product>()
            .Property(b => b.ArticleNo)
            .IsRequired(false);


        //Price 

        modelBuilder.Entity<Price>()
            .HasKey(b => b.PriceId);
        modelBuilder.Entity<Price>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<Price>()
            .Property(b => b.LastUpdated)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnUpdate()
            .IsRequired();
        modelBuilder.Entity<Price>()
            .Property(b => b.Code)
            .IsRequired(false);
        modelBuilder.Entity<Price>()
            .Property(b => b.Amount);
        modelBuilder.Entity<Price>()
            .Property(b => b.ProductId)
            .IsRequired(true);




        //ProductType 

        modelBuilder.Entity<ProductType>()
            .HasKey(b => b.ProductTypeId);
        modelBuilder.Entity<ProductType>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<ProductType>()
            .Property(b => b.LastUpdated)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnUpdate()
            .IsRequired();


        //Vendor 
        modelBuilder.Entity<Vendor>()
            .HasKey(b => b.VendorId);
        modelBuilder.Entity<Vendor>()
          .Property(b => b.Created)
          .HasDefaultValueSql("getdate()")
          .ValueGeneratedOnAdd()
          .IsRequired()
          .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<Vendor>()
            .Property(b => b.LastUpdated)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnUpdate()
            .IsRequired();

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