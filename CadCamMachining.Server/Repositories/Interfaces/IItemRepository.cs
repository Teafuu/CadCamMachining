using CadCamMachining.Server.Models;

namespace CadCamMachining.Server.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllAsync();
        Task<Item> GetByIdAsync(string id);
        Task<List<Item>> GetAllByItemType(string itemTypeId);
        Task CreateAsync(Item item);
        Task UpdateAsync(string id, Item item);
        Task DeleteAsync(string id);
        Task DeleteAllItemsByItemTypeId(string id);
    }
}
