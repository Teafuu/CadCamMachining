using CadCamMachining.Shared.Models;

namespace CadCamMachining.Server.Hub
{
    public interface IItemHub
    {
        Task SendItemUpdate(List<ItemDto> updatedItemsDto);

        Task SendItemTypeUpdate(ItemTypeDto updatedItemTypeDto);

        Task SendItemTypeDeleted(string id);

        Task SendItemDeleted(List<ItemDto> deletedItems);
    }
}
