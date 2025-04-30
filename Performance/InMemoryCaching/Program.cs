// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Caching.Memory;

IMemoryCache cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024});
string key = "UserProfiles";

for(int i = 0; i < 3; i++)
{
    if(!cache.TryGetValue(key, out List<string> users)) 
    {
        Console.WriteLine("Cache Miss. fetching user profiles");
        users = FetchUsers();

        cache.Set(key, users, 
            new MemoryCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3),
                Size = 1
             });
        Console.WriteLine("users added to the cache");
    }
    else 
    {
        Console.WriteLine("found users in the cache");
    }

    Console.WriteLine($"Cached data: {string.Join(", ", users)}");

    Thread.Sleep(2500); 
}

Console.WriteLine("clearing the cache");
cache.Remove(key);

List<string> FetchUsers() 
{
    return new List<string> {"shashank", "Nishu", "Addy"};
}

