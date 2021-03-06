
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class MyContext:DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<InfoRequest> InfoRequests { get; set; }
        public virtual DbSet<InfoRequestReply> InfoRequestReplys { get; set; }
        public virtual DbSet<Nation> Nations { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> Products_Categories { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public MyContext(DbContextOptions<MyContext> options)
        : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(builder =>
            {
                builder.HasQueryFilter(x => !x.IsDeleted);
                builder.HasKey(x => x.Id);
                builder.ToTable("Account");

                builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
                builder.Property(x => x.Password).IsRequired().HasMaxLength(18);
                builder.Property(x => x.AccountType).IsRequired();

            });

            modelBuilder.Entity<Brand>(builder =>
            {
                builder.HasQueryFilter(x => !x.IsDeleted);

                builder.HasKey(x => x.Id);
                builder.ToTable("Brand");
                builder.Property(x => x.AccountId).IsRequired();


                builder.HasOne(x=>x.Account)
                    .WithOne(x=>x.Brand)
                    .HasForeignKey<Brand>(x=>x.AccountId);

            });
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasQueryFilter(x => !x.IsDeleted);

                builder.HasKey(x => x.Id);
                builder.ToTable("User");
                builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
                builder.Property(x => x.LastName).IsRequired().HasMaxLength(255);
                builder.Property(x => x.AccountId).IsRequired().HasColumnName("AccountId");

                builder.HasOne(x => x.Account)
                    .WithOne(x => x.User)
                    .HasForeignKey<User>(x => x.AccountId);
                
            });
            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasQueryFilter(x => !x.IsDeleted);

                builder.HasKey(x => x.Id);
                builder.ToTable("Product");
                builder.Property(x => x.BrandId).IsRequired().HasColumnName("BrandId");
                builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
                builder.Property(x => x.ShortDescription).IsRequired().HasMaxLength(255);
                builder.Property(x => x.Price).HasColumnType("decimal(18,2)")
                     .HasConversion<decimal>().IsRequired();



                builder.HasOne(x => x.Brand)
                    .WithMany(x => x.Products)
                    .HasForeignKey(x => x.BrandId);

                builder.HasMany(x => x.InfoRequests)
                    .WithOne(x => x.Product)
                    .HasForeignKey(x => x.ProductId);

            });
            
            modelBuilder.Entity<ProductCategory>(builder =>
            {
                builder.HasQueryFilter(x => !x.IsDeleted);

                builder.ToTable("Product_Category");

                builder.HasKey(x => new { x.CategoryId, x.ProductId });

                //builder.HasOne(x => x.Product)
                //    .WithMany(x => x.Product_Categories)
                //    .HasForeignKey(x => x.ProductId).HasConstraintName("FK_ProductCategory");
                //builder.HasOne(x=>x.Category)
                //    .WithMany(x=>x.Product_Categories)
                //    .HasForeignKey(x=>x.CategoryId).HasConstraintName("FK_CategoryProduct");
            });

            modelBuilder.Entity<Category>(builder =>
            {
                builder.HasQueryFilter(x => !x.IsDeleted);

                builder.ToTable("Category");

                builder.Property(x=>x.Name).IsRequired();

            });

            modelBuilder.Entity<InfoRequest>(builder =>
            {
                builder.HasQueryFilter(x => !x.IsDeleted);

                builder.ToTable("InfoRequest");
                builder.Property(x => x.City).HasColumnName("Citta").HasMaxLength(189);
                builder.Property(x=>x.UserId).IsRequired(false).HasColumnName("UserId");
                builder.Property(x=>x.ProductId).IsRequired();
                builder.Property(x=>x.Name).IsRequired();
                builder.Property(x=>x.LastName).IsRequired();
                builder.Property(x=>x.Email).IsRequired();
                builder.Property(x => x.PhoneNumber).HasColumnName("Telefono").HasMaxLength(15);
                builder.Property(x=>x.Cap).HasMaxLength(18);
                builder.Property(x => x.InsertDate).HasConversion<DateTime>();

                builder.HasOne(x=>x.User)
                    .WithMany(x=>x.InfoRequests)
                    .HasForeignKey(x=>x.UserId);

                
                

            });

            modelBuilder.Entity<InfoRequestReply>(builder =>
            {
                builder.HasQueryFilter(x => !x.IsDeleted);

                builder.ToTable("InfoRequestReply");


                builder.HasOne(x=>x.InfoRequest)
                    .WithMany(x=>x.InfoRequestReplys)
                    .HasForeignKey(x=>x.InfoRequestId);

                builder.HasOne(x=>x.Account)
                    .WithMany(x=>x.InfoRequestReplies)
                    .HasForeignKey(x=>x.AccountId);
            });
            modelBuilder.Entity<Nation>(builder =>
            {
                builder.HasQueryFilter(x => !x.IsDeleted);

                builder.ToTable("Nation");

                builder.Property(x=>x.Name).IsRequired();

                builder.HasMany(x => x.InfoRequests)
                    .WithOne(x => x.Nation)
                    .HasForeignKey(x => x.NationId);
            });
        }



        public void DetachAllEntities()
        {
            var changedEntriesCopy = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

    }
}
