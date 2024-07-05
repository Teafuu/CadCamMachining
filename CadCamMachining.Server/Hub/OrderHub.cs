using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace CadCamMachining.Server.Hub
{
    public class OrderHub : Hub<IOrderHub>
    {
        // Optional: You can add methods to handle client connections, disconnections, etc.

        // Example method to broadcast order updates to clients
        public async Task SendOrderUpdate(ICollection<OrderDto> updatedOrders)
        {
            await Clients.All.SendOrderUpdate(updatedOrders);
        }
    }
}
