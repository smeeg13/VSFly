using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly
{
    public class VsflyContext : DbContext
    {

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Passenger> Passengers { get; set; }

        public DbSet<Pilot> Pilots { get; set; }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        public static string ConnectionString { get; set; } = @"Server=(localDB)\MSSQLLocalDB;Database=VsflyLimaSolliard;" +
                                                  "Trusted_Connection=True;App=VSFly;MultipleActiveResultSets=true";

        public VsflyContext()
        {
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(ConnectionString);
            options.UseLoggerFactory(MyLoggerFactory).EnableSensitiveDataLogging();

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Booking>().HasKey(x => new { x.FlightNo, x.PassengerID});

     
        }

    }
}
