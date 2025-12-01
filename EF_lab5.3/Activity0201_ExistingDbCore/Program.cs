using Activity0201_ExistingDbCore;
using EF_Activity001;
using InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Azure.Core.HttpHeader;

public class Program
{
    static IConfigurationRoot _configuration;
    static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

    static void Main(string[] args)
    {
        BuildOptions();
        Console.WriteLine(_configuration.GetConnectionString("InventoryManager"));
        DeleteAllItems();
        InsertItems();
        UpdateItems();
        ListInventory();
    }

    static void BuildOptions()
    {
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
    }

    static void UpdateItems()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var items = db.Items.ToList();

            foreach (var item in items)
            {
                item.LastModifiedUserId = 1;
                item.CurrentOrFinalPrice = 9.99M;
            }

            db.Items.UpdateRange(items);
            db.SaveChanges();
        }
    }

    static void DeleteAllItems()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var items = db.Items.ToList();

            foreach (var item in items)
            {
                item.LastModifiedUserId = 1;
            }

            db.Items.RemoveRange(items);
            db.SaveChanges();
        }
    }

    static void InsertItems()
    {
        var items = new List<Item>() {
            new Item() { Name = "Top Gun", IsActive = true, Description= "I feelthe need, the need for speed", Notes = "Notes" },
            new Item() { Name = "Batman Begins", IsActive = true, Description = "You either die the hero or live longenough to see yourself become the villain", Notes = "Notes" },
            new Item() { Name = "Inception", IsActive = true, Description = "Youmustn't be afraid to dream a little bigger", Notes = "Notes" },
            new Item() { Name = "Star Wars: The Empire Strikes Back", IsActive = true, Description = "He will join us or die, master", Notes = "Notes" },
            new Item() { Name = "Remember the Titans", IsActive = true, Description = "Attitude reflects leadership", Notes = "Notes" }
        };

        foreach (var item in items)
        {
            item.CreatedByUserId = 1;
        }

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