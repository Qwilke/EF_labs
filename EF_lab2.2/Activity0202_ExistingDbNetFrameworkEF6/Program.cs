using EF_Activity01_NetFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activity0202_ExistingDbNetFrameworkEF6
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AdventureWorks2019Entities())
            {
                var people = db.People.OrderByDescending(x => x.LastName).Take(20);

                foreach (var person in people)
                {
                    Console.WriteLine($"{person.FirstName} {person.LastName}");
                }

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
