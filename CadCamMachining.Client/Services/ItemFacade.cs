using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Net.Http.Json;

public class ItemFacade : IAsyncDisposable
{
    private readonly ILogger<ItemFacade> _logger;
    private readonly HttpClient _httpClient;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);
    private readonly HubConnection _hubConnection;
    private DateTime _itemsLastFetched;
    private DateTime _itemTypesLastFetched;

    private readonly object _itemsLock = new();
    private readonly object _itemTypesLock = new();

    public event EventHandler<List<ItemDto>> ItemsUpdated;
    public event EventHandler<List<ItemTypeDto>> ItemTypesUpdated;

    public readonly Dictionary<string, ItemTypeDto> ItemTypes = new();
    public readonly Dictionary<string, List<ItemDto>> Items = new();

    public ItemFacade(ILogger<ItemFacade> logger, HttpClient httpClient, NavigationManager navigationManager)
    {
        _logger = logger;
        _httpClient = httpClient;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/itemHub"))
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<List<ItemDto>>("SendItemUpdate", UpdateItemsInCache);
        _hubConnection.On<ItemTypeDto>("SendItemTypeUpdate", UpdateItemTypesInCache);
        _hubConnection.On<string>("SendItemTypeDeleted", DeleteItemTypeInCache);
        _hubConnection.On<List<ItemDto>>("SendItemDeleted", DeleteItemsInCache);

        _hubConnection.StartAsync().ConfigureAwait(false);
    }

    public async Task<ItemDto> SaveItem(ItemDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("api/items", item);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ItemDto>();
    }

    public async Task CreateItemType(ItemTypeDto itemType)
    {
        _logger.LogInformation($"Attempting to save {itemType.Name} with these properties: {itemType?.Properties?.Count()}");
        await _httpClient.PostAsJsonAsync("api/itemtypes", itemType);
    }

    public async Task UpdateItem(ItemDto item)
    {
        _logger.LogInformation($"Attempting to save {item.Name} with these properties: {item?.PropertyValues?.Count()}");
        await _httpClient.PutAsJsonAsync($"api/items/{item.Id}", item);
    }

    public async Task UpdateItemType(ItemTypeDto itemType)
    {
        _logger.LogInformation($"Attempting to save {itemType.Name} with these properties: {itemType?.Properties?.Count()}");
        await _httpClient.PutAsJsonAsync($"api/itemtypes/{itemType.Id}", itemType);
    }

    public async Task DeleteItemType(string itemTypeId)
    {
        await _httpClient.DeleteAsync($"api/ItemTypes/{itemTypeId}");
    }

    public async Task DeleteItem(string itemId)
    {
        await _httpClient.DeleteAsync($"api/items/{itemId}");
    }

    public async Task<List<ItemDto>> GetItemsAsync(ItemTypeDto itemType)
    {
        lock (_itemsLock)
        {
            if (Items.ContainsKey(itemType.Id) && DateTime.Now - _itemsLastFetched < _cacheDuration)
            {
                _logger.LogInformation($"Grabbing cached {itemType.Name} by last fetched: {_itemsLastFetched} cacheDuration: {_cacheDuration}");
                return Items[itemType.Id];
            }
        }
        _logger.LogInformation($"API call to get {itemType.Name} due to last fetched: {_itemsLastFetched} cacheDuration: {_cacheDuration}");

        var items = await _httpClient.GetFromJsonAsync<List<ItemDto>>($"api/items/byType/{itemType.Id}") ?? new List<ItemDto>();

        lock (_itemsLock)
        {
            if (!Items.ContainsKey(itemType.Id))
            {
                Console.WriteLine($"Creating new ItemType in Facade: {itemType.Id} {itemType.Name}");
                Items[itemType.Id] = new List<ItemDto>();
            }
            Items[itemType.Id].Clear();
            Items[itemType.Id].AddRange(items);
            _itemsLastFetched = DateTime.Now;
        }

        return Items[itemType.Id];
    }

    public async Task<List<ItemTypeDto>> GetItemTypesAsync()
    {
        try
        {
            lock (_itemTypesLock)
            {
                if (ItemTypes.Values.Count > 0 && DateTime.Now - _itemTypesLastFetched < _cacheDuration)
                {
                    return ItemTypes.Values.ToList();
                }
            }

            var itemTypes = await _httpClient.GetFromJsonAsync<List<ItemTypeDto>>("api/itemtypes") ?? new List<ItemTypeDto>();

            lock (_itemTypesLock)
            {
                ItemTypes.Clear();
                foreach (var type in itemTypes)
                {
                    ItemTypes[type.Id] = type;
                }
                _itemTypesLastFetched = DateTime.Now;
            }

            return ItemTypes.Values.ToList();
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "Network error occurred while fetching item types.");
            return new List<ItemTypeDto>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while fetching item types.");
            return new List<ItemTypeDto>();
        }
    }

    public void ClearCache()
    {
        lock (_itemsLock)
        {
            Items.Clear();
        }
        lock (_itemTypesLock)
        {
            ItemTypes.Clear();
        }
    }

    private void UpdateItemsInCache(List<ItemDto> updatedItems)
    {
        lock (_itemsLock)
        {
            foreach (var updatedItem in updatedItems)
            {
                if (Items.TryGetValue(updatedItem.ItemTypeId, out var itemList))
                {
                    var itemIndex = itemList.FindIndex(item => item.Id == updatedItem.Id);
                    if (itemIndex != -1)
                    {
                        itemList[itemIndex] = updatedItem;
                    }
                    else
                    {
                        itemList.Add(updatedItem);
                    }
                    ItemsUpdated?.Invoke(this, itemList);
                }
            }
        }
    }

    private void UpdateItemTypesInCache(ItemTypeDto updatedItemType)
    {
        Console.WriteLine($"ItemType is being updated in facade with property count: {updatedItemType.Name} {updatedItemType.Properties.Count()}");
        lock (_itemTypesLock)
        {
            ItemTypes[updatedItemType.Id] = updatedItemType;
            ItemTypesUpdated?.Invoke(this, ItemTypes.Values.ToList());
        }
    }

    private void DeleteItemTypeInCache(string itemTypeId)
    {
        lock (_itemTypesLock)
        {
            ItemTypes.Remove(itemTypeId);
            ItemTypesUpdated?.Invoke(this, ItemTypes.Values.ToList());
        }
    }

    private void DeleteItemsInCache(ICollection<ItemDto> deletedItems)
    {
        lock (_itemsLock)
        {
            foreach (var deletedItem in deletedItems)
            {
                if (Items.TryGetValue(deletedItem.ItemTypeId, out var itemList))
                {
                    var itemIndex = itemList.FindIndex(item => item.Id == deletedItem.Id);
                    if (itemIndex != -1)
                    {
                        itemList.RemoveAt(itemIndex);
                    }
                    ItemsUpdated?.Invoke(this, itemList);
                }
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}