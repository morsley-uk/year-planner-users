using Microsoft.EntityFrameworkCore;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System;

namespace Morsley.UK.YearPlanner.Users.Persistence.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Address> Addresses { get; set; }

        //public DbSet<Country> Countries { get; set; }

        //public DbSet<Email> Emails { get; set; }

        //public DbSet<Phone> Phones { get; set; }

        public DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserAddress>().HasKey(x => new { x.UserId, x.AddressId });
            modelBuilder
                .Entity<User>()
                .Property(u => u.Sex)
                .HasConversion(s => s.ToString(), s => (Sex)Enum.Parse(typeof(Sex), s));
        }
    }
}