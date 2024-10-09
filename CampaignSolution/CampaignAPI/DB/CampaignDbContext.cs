using CampaignService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace CampaignAPI.DB
{
    public class CampaignDbContext : DbContext
    {
        public DbSet<Agent> Agents { get; set; }
        public CampaignDbContext(DbContextOptions<CampaignDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>()
                .Property(a => a.ID)
                .IsRequired()
                .ValueGeneratedNever();

            modelBuilder.Entity<Agent>()
                .Property(a => a.Username)
                .IsRequired();

            modelBuilder.Entity<Agent>()
                .Property(a => a.Password)
                .IsRequired();

            modelBuilder.Entity<Reward>()
                .HasKey(_ => _.Id);

            modelBuilder.Entity<Campaign>().HasData(
                new Campaign
                {
                    Id = 1,
                    CampaignName = "Big Discount Campaign",
                    StartDate = DateTime.Today,
                    EndDate = new DateTime(2024, 12, 31, 23, 59, 59),
                    Discount = 20,
                    CampaignType = CampaignService.Enums.CampaignType.Discount
                }
            );

            modelBuilder.Entity<ProductDB>()
                .ToTable("products")
                .HasKey(_ => _.Id);

            modelBuilder.Entity<ProductDB>().HasData(
                new ProductDB
                {
                    Id = 1,
                    Name = "Mobile phone Samsung S24",
                    Price = 1500

                },
                new ProductDB
                {
                    Id = 2,
                    Name = "Mobile phone Samsung S23",
                    Price = 1000

                },
                new ProductDB
                {
                    Id = 3,
                    Name = "Laptop Acer Nitro",
                    Price = 2000

                },
                new ProductDB
                {
                    Id = 4,
                    Name = "Mobile phone Samsung S22",
                    Price = 500

                },
                new ProductDB
                {
                    Id = 5,
                    Name = "Sim Card",
                    Price = 5

                }
            );

            modelBuilder.Entity<Purchase>()
                .HasKey(_ => _.Id);
        }
    }
}
