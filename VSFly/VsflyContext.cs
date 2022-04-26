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
            builder.Entity<Booking>().HasKey(x => new { x.flightNo, x.passengerId });
        }

    }

    public class Pilot : Employee
    {
        public int flightHours { get; set; }
  
        public string flightSchool { get; set; }
        public DateTime LicenseDate { get; set; }
        public virtual List<Flight> flights { get; }

    }

    public class Employee : Person
    {
        [Required]
        public string passportNumber{ get; set; }
        [Required]
        public double salary { get; set; }

    }

    public class Person
    {
        [Required]
        public DateTime birthday { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string fullName { get; set; }

        public string givenName { get; set; }
        [Key]
        public int PersonId { get; set; }
        public string surname { get; set; }

    }

    public class Passenger : Person
    {
        public DateTime customerSince { get; set; }
        [Required]
        public string status { get; set; }

        public virtual List<Booking> bookings { get; }
    }

    public class Booking
    {
       
        public int flightNo { get; set; }
     
        public int passengerId { get; set; }
        public virtual Flight flight { get; set; }
        public virtual Passenger passenger { get; set; }
    }

    public class Flight
    {
        [Key]
        public int flightNo { get; set; }

        public virtual List<Booking> bookings { get; }

        [Required]
        public string airlineName { get; set; }
        [Required]
        public int copilotId { get; set; }

        [Required]
        public DateTime date { get; set; }

        [Required]
        public string destination { get; set; }
        [Required]
        public string departure { get; set; }


        public int freeSeats { get; set; }

        public string memo { get; set; }
        [Required]
        public Boolean nonSmokingFlight { get; set; }
        [Required]
        public int pilotId { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public int seat { get; set; }
        [Required]
        public Boolean strikebound { get; set; }
        [Required]
        public string timestamp { get; set; }

        [Required]
        public Boolean utilization { get; set; }
    }
}
