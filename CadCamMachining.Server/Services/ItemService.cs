using AutoMapper;
using CadCamMachining.Server.Hub;
using CadCamMachining.Server.Models;
using CadCamMachining.Server.Repositories.Interfaces;
using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;

namespace CadCamMachining.Server.Services
{
    public class ItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemTypeRepository _itemTypeRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<ItemHub, IItemHub> _hubContext;

        public ItemService(IItemRepository itemRepository, IItemTypeRepository itemTypeRepository, IMapper mapper, IHubContext<ItemHub, IItemHub> hubContext)
        {
            _itemRepository = itemRepository;
            _itemTypeRepository = itemTypeRepository;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<ItemDto> CreateItemAsync(ItemDto itemDto)
        {
            var itemType = await _itemTypeRepository.GetByIdAsync(itemDto.ItemTypeId);
            if (itemType == null)
            {
                throw new Exception("ItemType does not exist");
            }

            foreach (var propertyValueDto in itemDto.PropertyValues)
            {
                var property = itemType.Properties.Find(p => p.Id == propertyValueDto.ItemPropertyId);
                if (property == null)
                {
                    throw new Exception($"ItemProperty {propertyValueDto.ItemPropertyId} does not exist in ItemType {itemDto.ItemTypeId}");
                }

                if (propertyValueDto.Id == string.Empty)
                {
                    propertyValueDto.Id = ObjectId.GenerateNewId().ToString();
                }
            }

            var item = _mapper.Map<Item>(itemDto);
            await _itemRepository.CreateAsync(item);
            var createdItem = _mapper.Map<ItemDto>(item);
            await _hubContext.Clients.All.SendItemUpdate(new List<ItemDto>(1){createdItem});

            return createdItem;
        }

        public async Task<ItemDto> GetItemByIdAsync(string id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            return _mapper.Map<ItemDto>(item);
        }

        public async Task<List<ItemDto>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return _mapper.Map<List<ItemDto>>(items);
        }

        public async Task<List<ItemDto>> GetAllByItemTypeAsync(string itemTypeId)
        {
            var items = await _itemRepository.GetAllByItemType(itemTypeId);
            return _mapper.Map<List<ItemDto>>(items);
        }

        public async Task UpdateItemAsync(string id, ItemDto itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            await _itemRepository.UpdateAsync(id, item);

            var updatedItemDto = _mapper.Map<ItemDto>(item);
            await _hubContext.Clients.All.SendItemUpdate(new List<ItemDto>(1){ updatedItemDto } );
        }

        public async Task DeleteItemAsync(ItemDto item)
        {
            await _itemRepository.DeleteAsync(item.Id);
            await _hubContext.Clients.All.SendItemDeleted(new List<ItemDto>(1) { item });

        }
    }
}
