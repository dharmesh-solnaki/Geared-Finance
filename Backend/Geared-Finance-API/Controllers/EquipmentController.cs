using Entities.DTOs;
using Entities.UtilityModels;
using Geared_Finance_API.Auth;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Utilities;

namespace Geared_Finance_API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class EquipmentController : BaseController
{
    private readonly IEquipmentService _equipmentService;

    public EquipmentController(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    [HttpGet("Categories")]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_VIEW)]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<FundingCategoryDTO> categories = await _equipmentService.GetEquipmentCategoriesAsync();
        if (!categories.Any())
        {
            return NoContent();
        }
        return Ok(categories);
    }

    [HttpPost("Equipments")]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_VIEW)]
    public async Task<IActionResult> GetAllEquipmentType(BaseModelSearchEntity searchModal)
    {
        BaseResponseDTO<EquipmentRepsonseDTO> responseDTO = await _equipmentService.GetAllEquipmentType(searchModal);
        if (!responseDTO.ResponseData.Any())
        {
            return NoContent();
        }
        return Ok(responseDTO);
    }

    [HttpPost]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_UPSERT, Constants.ID_TYPE)]
    public async Task<IActionResult> Upsert(EquipmentTypeDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        await _equipmentService.UpsertAsync(model);
        return Ok();
    }
    [HttpDelete]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_DELETE)]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        ValidateId(id);
        bool isDeleted = await _equipmentService.DeleteEuipmentTypeAsync(id);
        if (!isDeleted) { return NotFound(Constants.RECORD_NOT_FOUND); }
        return Ok();
    }

}
