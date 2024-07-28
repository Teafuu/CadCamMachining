using AutoMapper;
using CadCamMachining.Server.Services;
using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CadCamMachining.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemTypesController : ControllerBase
    {
        private readonly ItemTypeService _service;
        private readonly IMapper _mapper;
        public ItemTypesController(ItemTypeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemTypeDto>>> GetItemTypes()
        {
            var itemTypes = await _service.GetAllItemTypesAsync();
            return _mapper.Map<List<ItemTypeDto>>(itemTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemTypeDto>> GetItemType(string id)
        {
            var itemType = await _service.GetItemTypeByIdAsync(id);

            if (itemType == null)
            {
                return NotFound();
            }

            return _mapper.Map<ItemTypeDto>(itemType);
        }

        [HttpPost]
        public async Task<ActionResult<ItemTypeDto>> CreateItemType(ItemTypeDto itemTypeDto)
        {
            var createdItemTypeDto = await _service.CreateItemTypeAsync(itemTypeDto);
            return CreatedAtAction(nameof(GetItemType), new { id = createdItemTypeDto.Id }, createdItemTypeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemType(string id, ItemTypeDto itemTypeDto)
        {
            var updatedItemType = await _service.UpdateItemTypeAsync(id, itemTypeDto);

            if (updatedItemType == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemType(string id)
        {
            await _service.DeleteItemTypeAsync(id);
            return NoContent();
        }
    }
}
