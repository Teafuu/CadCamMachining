using AutoMapper;
using CadCamMachining.Server.Hub;
using CadCamMachining.Server.Models;
using CadCamMachining.Server.Models.Properties;
using CadCamMachining.Server.Repositories.Interfaces;
using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;

namespace CadCamMachining.Server.Services
{
    public class ItemTypeService
    {
        private readonly ILogger<ItemTypeService> _logger;
        private readonly IItemTypeRepository _itemTypeRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IHubContext<ItemHub, IItemHub> _hubContext;

        private readonly IMapper _mapper;

        public ItemTypeService(ILogger<ItemTypeService> logger, IItemTypeRepository itemTypeItemTypeRepository, IItemRepository itemRepository, IMapper mapper, IHubContext<ItemHub, IItemHub> hubContext)
        {
            _logger = logger;
            _itemTypeRepository = itemTypeItemTypeRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<ItemTypeDto> CreateItemTypeAsync(ItemTypeDto itemTypeDto)
        {
            itemTypeDto.ChildConnections.ForEach(x => x.Id = ObjectId.GenerateNewId().ToString());
            itemTypeDto.ParentConnections.ForEach(x => x.Id = ObjectId.GenerateNewId().ToString());

            var itemType = _mapper.Map<ItemType>(itemTypeDto);
            await _itemTypeRepository.CreateAsync(itemType);
            var createdItemTypeDto = _mapper.Map<ItemTypeDto>(itemType);
            await _hubContext.Clients.All.SendItemTypeUpdate(createdItemTypeDto);

            return createdItemTypeDto;
        }

        public async Task<ItemTypeDto> GetItemTypeByIdAsync(string id)
        {
            var item = await _itemTypeRepository.GetByIdAsync(id);
            return _mapper.Map<ItemTypeDto>(item);
        }

        public async Task<List<ItemTypeDto>> GetAllItemTypesAsync()
        {
            var items = await _itemTypeRepository.GetAllAsync();
            return _mapper.Map<List<ItemTypeDto>>(items);
        }

        public async Task<ItemTypeDto> UpdateItemTypeAsync(string id, ItemTypeDto itemTypeDto)
        {
            var existingItemType = await _itemTypeRepository.GetByIdAsync(id);
            itemTypeDto.ChildConnections.ForEach(x =>
            {
                if(x.Id == string.Empty)
                    x.Id = ObjectId.GenerateNewId().ToString();
            });

            if (existingItemType == null)
            {
                return null;
            }

            var existingPropertyIds = existingItemType.Properties?.Select(p => p.Id).ToList() ?? new List<string>();
            var updatedPropertyIds = itemTypeDto.Properties?.Select(p => p.Id).ToList() ?? new List<string>();

            var itemType = _mapper.Map(itemTypeDto, existingItemType);

            // Generate IDs for new properties
            itemType.Properties?.ForEach(x =>
            {
                if (x.Id == string.Empty)
                {
                    x.Id = ObjectId.GenerateNewId().ToString();
                }
            });

            // Update the ItemType
            await _itemTypeRepository.UpdateAsync(id, itemType);

            var addedProperties = itemType.Properties.Where(x => !existingPropertyIds.Contains(x.Id)).ToList();
            var removedPropertyIds = existingPropertyIds.Except(updatedPropertyIds).ToList();

            // Update the corresponding items
            await UpdateItemsForItemType(id, addedProperties, removedPropertyIds);

            var updatedItemTypeDto = _mapper.Map<ItemTypeDto>(itemType);
            await _hubContext.Clients.All.SendItemTypeUpdate(updatedItemTypeDto);

            return updatedItemTypeDto;
        }

        private async Task UpdateItemsForItemType(string itemTypeId, List<ItemProperty> addedProperties, List<string> removedPropertyIds)
        {
            var items = await _itemRepository.GetAllByItemType(itemTypeId);
            var hasChanged = false;
            foreach (var item in items)
            {
                // Add new properties
                foreach (var addedProperty in addedProperties)
                {
                    if (!item.PropertyValues.Any(pv => pv.ItemPropertyId == addedProperty.Id))
                    {
                        item.PropertyValues.Add(GetProperty(addedProperty, ObjectId.GenerateNewId().ToString()));
                        hasChanged = true;
                    }
                }

                // Remove properties that are no longer defined
                var count = item.PropertyValues.RemoveAll(pv => removedPropertyIds.Contains(pv.ItemPropertyId));

                if (hasChanged || count > 0)
                {
                    await _itemRepository.UpdateAsync(item.Id, item);
                    hasChanged = true;
                }
                // Update the item in the database
            }

            if (hasChanged)
            {
                var itemsDtos = _mapper.Map<List<ItemDto>>(items);
                await _hubContext.Clients.All.SendItemUpdate(itemsDtos);
            }
        }

        private ItemPropertyValue GetProperty(ItemProperty type, string id) => type.PropertyType switch
        {
            PropertyType.Bool => new BoolPropertyValue { ItemPropertyId = type.Id, Id = id},
            PropertyType.DateTime => new DateTimePropertyValue { ItemPropertyId = type.Id, Id = id },
            PropertyType.Enum => new EnumPropertyValue { ItemPropertyId = type.Id, Id = id },
            PropertyType.String => new StringPropertyValue { ItemPropertyId = type.Id, Id = id },
            _ => throw new InvalidDataException()
        };
        public async Task DeleteItemTypeAsync(string id)
        {
            await _itemTypeRepository.DeleteAsync(id);
            await _itemRepository.DeleteAllItemsByItemTypeId(id);

            await _hubContext.Clients.All.SendItemTypeDeleted(id);
        }
    }
}
