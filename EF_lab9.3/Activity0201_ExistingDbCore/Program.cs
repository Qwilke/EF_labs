using Activity0201_ExistingDbCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EF_Activity001;
using InventoryModels;
using InventoryModels.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    static IConfigurationRoot _configuration;
    static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;
    private static MapperConfiguration _mapperConfig;
    private static IMapper _mapper;
    private static IServiceProvider _serviceProvider;

    static void Main(string[] args)
    {
        BuildOptions();
        BuildMapper();
        Console.WriteLine(_configuration.GetConnectionString("InventoryManager"));
        DeleteAllItems();
        InsertItems();
        UpdateItems();
        ListInventory();
        //GetItemsForListingLinq();
        ListInventoryWithProjection();
        ListCategoriesAndColors();
    }

    static void ListCategoriesAndColors()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var results = db.Categories.Select(x => new CategoryDto
            {
                Category = x.Name,
                CategoryColor = new CategoryColorDto
                {
                    Color = x.CategoryColor.ColorValue
                }
            });

            foreach (var c in results)
            {
                Console.WriteLine($"{c.Category} | {c.CategoryColor.Color}");
            }
        }
    }

    static void ListInventoryWithProjection()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var items = db.Items.Take(5)
                    .OrderBy(x => x.Name)
                    .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                    .ToList();
            items.ForEach(x => Console.WriteLine($"New Item: {x}"));
        }
    }

    static void GetItemsForListingLinq()
    {
        var minDateValue = new DateTime(2010, 1, 1);
        var maxDateValue = new DateTime(2026, 1, 1);
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var results = db.Items.Select(x => new GetItemsForListingWithDateDto
            {
                CreatedDate = x.CreatedDate,
                CategoryName = x.Category.Name,
                Description = x.Description,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
                Name = x.Name,
                Notes = x.Notes
            })
                .Where(x => x.CreatedDate >= minDateValue && x.CreatedDate <= maxDateValue)
                .OrderBy(y => y.CategoryName)
                .ThenBy(z => z.Name)
                .ToList();

            foreach (var item in results)
            {
                Console.WriteLine($"ITEM {item.CategoryName}| {item.Name} - { item.Description}");
            }
        }
    }

    static void BuildOptions()
    {
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
    }

    static void BuildMapper()
    {
        var services = new ServiceCollection();
        services.AddAutoMapper(typeof(InventoryMapper));
        _serviceProvider = services.BuildServiceProvider();
        _mapper = _serviceProvider.GetRequiredService<IMapper>();
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
            var result = _mapper.Map<List<Item>, List<ItemDto>>(items);
            result.ForEach(x => Console.WriteLine($"New Item: {x}"));
        }
    }
}