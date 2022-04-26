using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly
{
    class VsflyContext : DbContext
    {

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Passenger> Passengers { get; set; }

        public DbSet<Pilot> Pilots { get; set; }

        public static string ConnectionString { get; set; } = @"Server=(localDB)\MSSQLLocalDB;Database=VsflyLimaSolliard";

        public VsflyContext()
        {
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(ConnectionString);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Booking>().HasKey(x => new { x.FlightNo, x.PassengerId });

            //Default Values
            builder.Entity<Flight>().Property(b => b.NonSmokingFlight).HasDefaultValue(true);

        }

    }
}
