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
        Console.WriteLine("Please Enter the partial First or Last Name, or the Person Type to search for:");
        var result = Console.ReadLine();
        int pageSize = 10;
        for (int pageNumber = 0; pageNumber < 10; pageNumber++)
        {
            Console.WriteLine($"Page {pageNumber + 1}");
            FilteredAndPagedResult(result, pageNumber, pageSize);
        }
    }

    static void FilteredAndPagedResult(string filter, int pageNumber, int pageSize)
    {
        using (var db = new AdventureWorks2019Context(_optionsBuilder.Options))
        {
            var searchTerm = filter.ToLower();
            var query = db.People.Where(x => x.LastName.ToLower().Contains(searchTerm) ||
                x.FirstName.ToLower().Contains(searchTerm) ||
                x.PersonType.ToLower().Equals(searchTerm))
            .OrderBy(x => x.LastName)
            .Skip(pageNumber * pageSize)
            .Take(pageSize);

            foreach (var person in query)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}, { person.PersonType}");
            }
        }
    }

    static void FilteredPeople(string filter)
    {
        using (var db = new AdventureWorks2019Context(_optionsBuilder.Options))
        {
            var searchTerm = filter.ToLower();
            var query = db.People.Where(x =>
                x.LastName.ToLower().Contains(searchTerm) ||
                x.FirstName.ToLower().Contains(searchTerm) ||
                x.PersonType.ToLower().Equals(searchTerm));

            foreach (var person in query)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}, {person.PersonType}");
            }
        }
    }

    static void ListPeopleThenOrderAndTake()
    {
        using (var db = new AdventureWorks2019Context(_optionsBuilder.Options))
        {
            var people = db.People.ToList().OrderByDescending(x => x.LastName);

            foreach (var person in people.Take(10))
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
            }
        }
    }

    static void QueryPeopleOrderedToListAndTake()
    {
        using (var db = new AdventureWorks2019Context(_optionsBuilder.Options))
        {
            var query = db.People.OrderByDescending(x => x.LastName);

            var result = query.Take(10);
            foreach (var person in result)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
            }
        }
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