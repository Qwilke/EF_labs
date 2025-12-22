using EF_Activity001;
using EF_Activity001.DTO;
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

        var input = string.Empty;
        Console.WriteLine("Would you like to view the sales report?");
        input = Console.ReadLine();
        if (input.StartsWith("y", StringComparison.OrdinalIgnoreCase))
        {
            //GenerateSalesReport();
            GenerateSalesReportToDTO();
        }
    }

    private static void GenerateSalesReportToDTO()
    {
        Console.WriteLine("What is the minimum amount of sales?");
        var input = Console.ReadLine();
        decimal filter = 0.0m;

        if (!decimal.TryParse(input, out filter))
        {
            Console.WriteLine("Bad input");
            return;
        }

        using (var db = new AdventureWorks2019Context(_optionsBuilder.Options))
        {
            var salesReportDetails = db.SalesPeople.Select(x => new SalesReportListingDto
            {
                BusinessEntityId = x.BusinessEntityId,
                FirstName = x.BusinessEntity.BusinessEntity.FirstName,
                LastName = x.BusinessEntity.BusinessEntity.LastName,
                SalesYtd = x.SalesYtd,
                Territories = x.SalesTerritoryHistories.Select(y => y.Territory.Name),
                TotalOrders = x.SalesOrderHeaders.Count(),
                TotalProductsSold = x.SalesOrderHeaders
                            .SelectMany(y => y.SalesOrderDetails).Sum(z => z.OrderQty)
            }).Where(srds => srds.SalesYtd > filter)
                .OrderBy(srds => srds.LastName)
                    .ThenBy(srds => srds.FirstName)
                        .ThenByDescending(srds => srds.SalesYtd);

            foreach (var srd in salesReportDetails)
            {
                Console.WriteLine(srd);
            }
        }
    }

    private static void GenerateSalesReport()
    {
        Console.WriteLine("What is the minimum amount of sales?");
        var input = Console.ReadLine();
        decimal filter = 0.0m;

        if (!decimal.TryParse(input, out filter))
        {
            Console.WriteLine("Bad input");
            return;
        }

        using (var db = new AdventureWorks2019Context(_optionsBuilder.Options))
        {
            var salesReportDetails = db.SalesPeople.Select(sp => new
            {
                beid = sp.BusinessEntityId,
                sp.BusinessEntity.BusinessEntity.FirstName,
                sp.BusinessEntity.BusinessEntity.LastName,
                sp.SalesYtd,
                Territories = sp.SalesTerritoryHistories
                            .Select(y => y.Territory.Name),
                OrderCount = sp.SalesOrderHeaders.Count(),
                TotalProductsSold = sp.SalesOrderHeaders
                        .SelectMany(y => y.SalesOrderDetails)
                        .Sum(z => z.OrderQty)
            }).Take(20)
            .Where(srds => srds.SalesYtd > filter)
                .OrderBy(srds => srds.LastName)
                    .ThenBy(srds => srds.FirstName)
                        .ThenByDescending(srds => srds.SalesYtd)
                .Take(20).ToList();

            foreach (var srd in salesReportDetails)
            {
                Console.WriteLine($"{srd.beid}| {srd.LastName}, {srd.FirstName} |" +
                    $"YTD Sales: {srd.SalesYtd} |" +
                    $"{string.Join(',', srd.Territories)} |" +
                    $"Order Count: {srd.OrderCount} |" +
                    $"Products Sold: {srd.TotalProductsSold}");
            }
        }
    }

    private static void ShowAllSalesPeopleUsingProjection()
    {
        using (var db = new AdventureWorks2019Context(_optionsBuilder.Options))
        {
            var salesPeople = db.SalesPeople
                .Select(x => new {
                    x.BusinessEntityId,
                    x.BusinessEntity.BusinessEntity.FirstName,
                    x.BusinessEntity.BusinessEntity.LastName,
                    x.SalesQuota,
                    x.SalesYtd,
                    x.SalesLastYear
                })
                .ToList();

            foreach (var sp in salesPeople)
            {
                Console.WriteLine($"BID: {sp.BusinessEntityId} | Name: {sp.LastName}" +
                    $", {sp.FirstName} | Quota: {sp.SalesQuota} | " +
                    $"YTD Sales: {sp.SalesYtd} | SalesLastYear {sp.SalesLastYear}");
            }
        }
    }

    private static void ShowAllSalesPeople()
    {
        using (var db = new AdventureWorks2019Context(_optionsBuilder.Options))
        {
            var salesPeople = db.SalesPeople
                .Include(x => x.BusinessEntity)
                .ThenInclude(y => y.BusinessEntity)
                .Take(20)
                .ToList();

            foreach (var sp in salesPeople)
            {
                Console.WriteLine($"{sp} | {sp.BusinessEntity.BusinessEntity.LastName}" +
                    $", {sp.BusinessEntity.BusinessEntity.FirstName}");
            }
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