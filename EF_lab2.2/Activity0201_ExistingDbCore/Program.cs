using EF_Activity001;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class Program
{
    static IConfigurationRoot _configuration;
    static DbContextOptionsBuilder<AdventureWorks2019Context> _optionsBuilder;

    private static void Main(string[] args)
    {
        BuildConfiguration();
        BuildOptions();
        ListPeople();
        Console.WriteLine("Hello, World!");
    }

    static void BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();
    }

    static void BuildOptions()
    {
        _optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
        _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AdventureWorks2019"));
    }

    static void ListPeople()
    {
        using (var db = new AdventureWorks2019Context(_optionsBuilder.Options))
        {
            var people = db.People.OrderByDescending(x => x.LastName).Take(20).ToList();

            foreach (var person in people)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
            }
        }
    }
}