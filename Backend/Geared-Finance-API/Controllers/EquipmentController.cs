using Entities.DTOs;
using Entities.UtilityModels;
using Geared_Finance_API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Utilities;

namespace Geared_Finance_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpGet("GetAllCategories")]
        [AuthorizePermission(ModuleConstants.SETTINGS, PermissionConstants.CAN_VIEW)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<FundingCategoryDTO> categories = await _equipmentService.GetEquipmentCategoriesAsync();
            if (!categories.Any())
            {
                return NoContent();
            }
            return Ok(categories);
        }

        [HttpPost("GetEquipments")]
        [AuthorizePermission(ModuleConstants.SETTINGS, PermissionConstants.CAN_VIEW)]
        public async Task<IActionResult> GetAllEquipmentType(BaseModelSearchEntity searchModal)
        {
            BaseRepsonseDTO<EquipmentRepsonseDTO> responseDTO = await _equipmentService.GetAllEquipmentType(searchModal);
            if (!responseDTO.responseData.Any())
            {
                return NoContent();
            }
            return Ok(responseDTO);
        }

        [HttpPost]
        [AuthorizePermission(ModuleConstants.SETTINGS, PermissionConstants.CAN_UPSERT, "Id")]
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
        [AuthorizePermission(ModuleConstants.SETTINGS, PermissionConstants.CAN_DELETE)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (id <= 0) return BadRequest();
            bool isDeleted = await _equipmentService.DeleteEuipmentTypeAsync(id);
            if (!isDeleted) { return NotFound(Constants.RECORD_NOT_FOUND); }
            return Ok();
        }

    }
}
