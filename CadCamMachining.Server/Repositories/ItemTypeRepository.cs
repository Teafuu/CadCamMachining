using AspNetCore.Identity.MongoDbCore.Infrastructure;
using CadCamMachining.Server.Models;
using CadCamMachining.Server.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CadCamMachining.Server.Repositories
{
    public class ItemTypeRepository : IItemTypeRepository
    {
        private readonly IMongoCollection<ItemType> _itemTypes;

        public ItemTypeRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _itemTypes = database.GetCollection<ItemType>("ItemTypes");
        }

        public async Task<List<ItemType>> GetAllAsync()
        {
            return await _itemTypes.Find(itemType => true).ToListAsync();
        }

        public async Task<ItemType> GetByIdAsync(string id)
        {
            return await _itemTypes.Find<ItemType>(itemType => itemType.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(ItemType itemType)
        {
            await _itemTypes.InsertOneAsync(itemType);
        }

        public async Task UpdateAsync(string id, ItemType itemType)
        {
            await _itemTypes.ReplaceOneAsync(itemType => itemType.Id == id, itemType);
        }

        public async Task DeleteAsync(string id)
        {
            await _itemTypes.DeleteOneAsync(itemType => itemType.Id == id);
        }
    }
}
