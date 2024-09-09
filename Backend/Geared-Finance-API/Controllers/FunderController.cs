using Entities.DTOs;
using Entities.UtilityModels;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace Geared_Finance_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunderController : BaseController
    {
        private readonly IFunderService _funderService;

        public FunderController(IFunderService funderService)
        {
            _funderService = funderService;
        }

        [HttpPost("Funders")]
        public async Task<IActionResult> GetFunders(BaseModelSearchEntity searchModel)
        {
            BaseRepsonseDTO<DisplayFunderDTO> funders = await _funderService.GetAllFunders(searchModel);
            if (!funders.responseData.Any())
            {
                return NoContent();
            }
            return Ok(funders);
        }

        [HttpGet]
        public async Task<IActionResult> GetFunder([FromQuery] int id)
        {
            ValidateId(id);
            FunderDTO funder = await _funderService.GetFunder(id);
            if (funder == null)
            {
                return NotFound();
            }
            return Ok(funder);
        }
        [HttpGet("FunderGuide")]
        public async Task<IActionResult> GetFunderGuide([FromQuery] int id)
        {
            ValidateId(id);
            FunderGuideTypeDTO funderGuide = await _funderService.GetFunderType(id);            
            return Ok(funderGuide);
        }

        [HttpPost("Funder")]
        //[AuthorizePermission(Constants.SETTINGS, Constants.CAN_UPSERT, Constants.ID_TYPE)]
        public async Task<IActionResult> UpsertFunder(FunderDTO funderDTO)
        {
            ValidateModel();
            int id =  await _funderService.UpsertFunder(funderDTO);
            return Ok(id);
        }

        [HttpPost("FundigGuide")]
        public async Task<IActionResult> UpsertFunderGuide(FunderGuideTypeDTO funderGuideTypeDTO)
        {
            ValidateModel();
           int id = await _funderService.UpsertFunderGuide(funderGuideTypeDTO);
            return Ok(id);
        }

    }
}
