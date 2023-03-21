using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductMicroService.Model;

namespace ProductMicroService.DBContexts
{
    public class ProductContext :DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<BuyerBid> BuyerBids { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    ProductCategoryId = 1,
                    ProductCategoryName = "Painting",
                    ProductCategoryDescription = "Painting",
                },
                new Category
                {
                    ProductCategoryId = 2,
                    ProductCategoryName = "Sculptor",
                    ProductCategoryDescription = "Sculptor",
                },
                new Category
                {
                    ProductCategoryId = 3,
                    ProductCategoryName = "Ornament",
                    ProductCategoryDescription = "Ornament",
                }
            );
        }

    }
}

