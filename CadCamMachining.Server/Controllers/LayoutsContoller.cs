using AutoMapper;
using CadCamMachining.Server.Models.Layouts;
using CadCamMachining.Server.Repositories.Interfaces;
using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
namespace CadCamMachining.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LayoutsController : ControllerBase
{
    private readonly ILayoutRepository _repository;
    private readonly IMapper _mapper;

    public LayoutsController(ILayoutRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LayoutDto>>> GetLayouts()
    {
        var layouts = await _repository.GetAllAsync();

        var layoutDtos = _mapper.Map<List<LayoutDto>>(layouts);
        return Ok(layoutDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LayoutDto>> GetLayout(string id)
    {
        var layout = await _repository.GetByIdAsync(id);

        if (layout == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<LayoutDto>(layout));
    }

    [HttpPost]
    public async Task<ActionResult<LayoutDto>> CreateLayout(LayoutDto layoutDto)
    {
        layoutDto.Components.ForEach(x =>
        {
            if (x.Id == string.Empty)
            {
                x.Id = ObjectId.GenerateNewId().ToString();
            }
        });

        var layout = _mapper.Map<Layout>(layoutDto);
        await _repository.CreateAsync(layout);
        return CreatedAtAction(nameof(GetLayout), new { id = layout.Id }, _mapper.Map<LayoutDto>(layout));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLayout(string id, LayoutDto layoutDto)
    {
        var existingLayout = await _repository.GetByIdAsync(id);

        if (existingLayout == null)
        {
            return NotFound();
        }

        foreach (var existingLayoutComponent in layoutDto.Components)
        {
            if (existingLayoutComponent.Id == string.Empty)
            {
                existingLayoutComponent.Id = ObjectId.GenerateNewId().ToString();
            }
        }

        var layout = _mapper.Map(layoutDto, existingLayout);
        await _repository.UpdateAsync(id, layout);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLayout(string id)
    {
        var existingLayout = await _repository.GetByIdAsync(id);

        if (existingLayout == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}