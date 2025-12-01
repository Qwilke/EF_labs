using Activity0201_ExistingDbCore;
using EF_Activity001;
using InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class Program
{
    static IConfigurationRoot _configuration;
    static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

    static void Main(string[] args)
    {
        BuildOptions();
        Console.WriteLine(_configuration.GetConnectionString("InventoryManager"));
        ListInventory();
    }

    static void BuildOptions()
    {
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
    }

    static void InsertItems()
    {
        var items = new List<Item>() {
            new Item() { Name = "Top Gun" },
            new Item() { Name = "Batman Begins"},
            new Item() { Name = "Inception" },
            new Item() { Name = "Star Wars: The Empire Strikes Back"},
            new Item() { Name = "Remember the Titans"}
        };

        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            db.AddRange(items);
            db.SaveChanges();
        }
    }

    static void ListInventory()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var items = db.Items.Take(5).OrderBy(x => x.Name).ToList();
            items.ForEach(x => Console.WriteLine($"New Item: {x.Name}"));
        }
    }
}