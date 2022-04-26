using System;
using System.Linq;

namespace VSFly
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello from VFLy!");

            var context = new VsflyContext();
            var e = context.Database.EnsureCreated();

            if (e)
            {
                Console.Write("DB created");
            }
            else
            {
                Console.Write("DB already exists");

            }
            Console.Write("Done");


            //Add
            Pilot p = new Pilot() { FlightHours = 10, LicenseDate = DateTime.Today, Birthday = DateTime.Today, Email = "meg@gmail.com", FullName = "Meg Solliard", PassportNumber = "NJ908ZTG8", Salary = 10000 };
            context.Pilots.Add(p);
            context.SaveChanges();
            Console.Write("Add Done");


            var pilotList = context.Pilots.ToList<Pilot>();

            //Edit
            Pilot pilotToUpdate = pilotList.Where(p => p.FullName == "Meg Solliard ")
                .FirstOrDefault<Pilot>();
            pilotToUpdate.FullName = "Meg Solliard Edited";
            context.SaveChanges();
            Console.Write("Update Done");


            //Delete
            context.Pilots.Remove(pilotList.ElementAt<Pilot>(0));
            context.SaveChanges();
            Console.Write("Remove Done");
        }
    }
}
