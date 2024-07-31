using Entities.DTOs;
using Entities.UtilityModels;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

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
        public async Task<IActionResult> GetAllEquipmentType(BaseModelSearchEntity searchModal)
        {
            BaseRepsonseDTO<EquipmentRepsonseDTO> responseDTO =await _equipmentService.GetAllEuqipmentType(searchModal);
            if (!responseDTO.responseData.Any())
            {
                return NoContent();
            }
            return Ok(responseDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(EquipmentTypeDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _equipmentService.UpsertAsync(model);
            return Ok();
        }

    }
}
