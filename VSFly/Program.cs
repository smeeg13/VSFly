using System;

namespace VSFly
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

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
            Pilot p = new Pilot();
            context.Pilots.Add(p);
            context.SaveChanges();

        }
    }
}
