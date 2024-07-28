using CadCamMachining.Server.Models;

namespace CadCamMachining.Server.Repositories.Interfaces
{
    public interface IItemTypeRepository
    {
        Task<List<ItemType>> GetAllAsync();
        Task<ItemType> GetByIdAsync(string id);
        Task CreateAsync(ItemType itemType);
        Task UpdateAsync(string id, ItemType itemType);
        Task DeleteAsync(string id);
    }
}
