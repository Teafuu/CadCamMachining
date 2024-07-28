using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace CadCamMachining.Server.Hub
{
    public class ItemHub : Hub<IItemHub>
    {
        // Optional: You can add methods to handle client connections, disconnections, etc.

        // Example method to broadcast order updates to clients
        public async Task SendItemUpdate(List<ItemDto> updatedItemsDto)
        {
            await Clients.All.SendItemUpdate(updatedItemsDto);
        }

        public async Task SendItemTypeUpdate(ItemTypeDto updatedItemTypeDto)
        {
            await Clients.All.SendItemTypeUpdate(updatedItemTypeDto);
        }

        public async Task SendItemTypeDeleted(string id)
        {
            await Clients.All.SendItemTypeDeleted(id);
        }

        public async Task SendItemDeleted(List<ItemDto> deletedItems)
        {
            await Clients.All.SendItemDeleted(deletedItems);
        }

    }
}
