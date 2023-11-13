using MongoDB.Driver;
using MongoDB.Entities;
using System.Text.Json;

namespace SearchService;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync(
            "SearchDB",
            MongoClientSettings.FromConnectionString(
                app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

        var items = await httpClient.GetItemsForSearchDb();

        Console.WriteLine($"{items.Count} items returned from the auction service");

        //if (count == 0)
        //{
        //    await SeedDb();
        //}
    }

    //private static async Task SeedDb()
    //{
    //    Console.WriteLine("No data - will attempt to seed");
    //    var itemData = await File.ReadAllTextAsync("Data/auctions.json");
    //    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    //    try
    //    {
    //        var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);
    //        if (items is null)
    //        {
    //            Console.WriteLine("No data found to seed the DB");
    //        }
    //        else
    //        {
    //            await DB.SaveAsync(items);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Could not seed Mongo database. Exception message: {ex.Message}");
    //    }
    //}
}
