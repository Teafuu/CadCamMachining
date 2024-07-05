using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

public class OrderFacade : IAsyncDisposable
{
    private readonly HttpClient _httpClient;
    private readonly Dictionary<Guid, OrderDto> _orderCache = new();
    private readonly List<OrderDto> _orders = new();
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);
    private readonly HubConnection _hubConnection;
    private DateTime _ordersLastFetched;

    public EventHandler<ICollection<OrderDto>> OrdersUpdated;

    public OrderFacade(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/orderHub"))
            .Build();

        _hubConnection.On<List<OrderDto>>("SendOrderUpdate", UpdateOrdersInCache);
        _hubConnection.StartAsync().ConfigureAwait(false);
    }

    public async Task<List<OrderDto>> GetOrdersAsync()
    {
        if (_orders.Count > 0 && DateTime.Now - _ordersLastFetched < _cacheDuration)
        {
            return _orders;
        }

        var orders = await _httpClient.GetFromJsonAsync<List<OrderDto>>("api/order") ?? new List<OrderDto>();
        _orders.Clear();
        _orders.AddRange(orders);
        _ordersLastFetched = DateTime.Now;

        foreach (var order in orders)
        {
            _orderCache[order.Id] = order;
        }

        return _orders;
    }

    public async Task<OrderDto> RefreshOrderAsync(Guid orderId)
    {
        var order = await _httpClient.GetFromJsonAsync<OrderDto>($"api/order/{orderId}");
        _orderCache[orderId] = order;

        var index = _orders.FindIndex(o => o.Id == orderId);
        if (index != -1)
        {
            _orders[index] = order;
        }
        else
        {
            _orders.Add(order);
        }

        return order;
    }

    public void ClearCache()
    {
        _orderCache.Clear();
        _orders.Clear();
    }

    private void UpdateOrdersInCache(List<OrderDto> updatedOrders)
    {
        foreach (var updatedOrder in updatedOrders)
        {
            _orderCache[updatedOrder.Id] = updatedOrder;

            var index = _orders.FindIndex(o => o.Id == updatedOrder.Id);
            if (index != -1)
            {
                _orders[index] = updatedOrder;
            }
            else
            {
                _orders.Add(updatedOrder);
            }
        }
        OrdersUpdated.Invoke(this, _orders);
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}