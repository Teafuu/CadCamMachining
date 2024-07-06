using AspNetCore.Identity.MongoDbCore.Infrastructure;
using CadCamMachining.Client;
using CadCamMachining.Server.Models;
using CadCamMachining.Server.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CadCamMachining.Server.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IMongoCollection<Item> _items;

        public ItemRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _items = database.GetCollection<Item>("Items");
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _items.Find(item => true).ToListAsync();
        }

        public async Task<List<Item>> GetAllByItemType(string itemTypeId)
        {
            return await _items.Find(x => x.ItemTypeId == itemTypeId).ToListAsync();
        }

        public async Task<Item> GetByIdAsync(string id)
        {
            return await _items.Find<Item>(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item item)
        {
            await _items.InsertOneAsync(item);
        }

        public async Task UpdateAsync(string id, Item item)
        {
            await _items.ReplaceOneAsync(item => item.Id == id, item);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<Item>.Filter.Or(
                Builders<Item>.Filter.ElemMatch(x => x.ChildConnections, c => c.ChildItemId == id),
                Builders<Item>.Filter.ElemMatch(x => x.ChildConnections, c => c.ParentItemId == id),
            Builders<Item>.Filter.ElemMatch(x => x.ParentConnections, c => c.ChildItemId == id),
            Builders<Item>.Filter.ElemMatch(x => x.ParentConnections, c => c.ParentItemId == id)
            );

            var update = Builders<Item>.Update.PullFilter(x => x.ChildConnections, c => c.ChildItemId == id || c.ParentItemId == id);

            await _items.UpdateManyAsync(filter, update);
            await _items.DeleteOneAsync(item => item.Id == id);
        }

        public async Task DeleteAllItemsByItemTypeId(string itemTypeId)
        {
            await _items.DeleteManyAsync(item => item.ItemTypeId == itemTypeId);
        }
    }
}
