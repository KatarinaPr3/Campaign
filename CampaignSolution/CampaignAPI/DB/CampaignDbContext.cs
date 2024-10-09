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
        }



    }
}
