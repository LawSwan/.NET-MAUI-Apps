using System.Collections.Concurrent;

namespace AssessmentApi.Models;

public sealed class ItemStore
{
    private readonly ConcurrentDictionary<string, Item> items = new();

    public IReadOnlyCollection<Item> GetAll() => items.Values.OrderBy(item => item.Id).ToArray();

    public Item Add(Item item)
    {
        items[item.Id] = item;
        return item;
    }
}