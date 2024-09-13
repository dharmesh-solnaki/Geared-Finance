using Entities.DTOs;
using Entities.UtilityModels;
using Geared_Finance_API.Auth;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Utilities;

namespace Geared_Finance_API.Controllers;

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
    [AuthorizePermission(Constants.FUNDERS, Constants.CAN_VIEW)]
    public async Task<IActionResult> GetFunders(BaseModelSearchEntity searchModel)
    {
        BaseResponseDTO<DisplayFunderDTO> funders = await _funderService.GetAllFunders(searchModel);
        if (!funders.ResponseData.Any())
        {
            return NoContent();
        }
        return Ok(funders);
    }

    [HttpGet]
    [AuthorizePermission(Constants.FUNDERS, Constants.CAN_VIEW)]
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
    [AuthorizePermission(Constants.FUNDERS, Constants.CAN_VIEW)]
    public async Task<IActionResult> GetFunderGuide([FromQuery] int id)
    {
        ValidateId(id);
        FunderGuideTypeDTO funderGuide = await _funderService.GetFunderType(id);
        return Ok(funderGuide);
    }

    [HttpPost("Funder")]
    [AuthorizePermission(Constants.FUNDERS, Constants.CAN_UPSERT, Constants.ID_TYPE)]
    public async Task<IActionResult> UpsertFunder(FunderDTO funderDTO)
    {
        ValidateModel();
        int id = await _funderService.UpsertFunder(funderDTO);
        return Ok(id);
    }

    [HttpPost("FundingGuide")]
    [AuthorizePermission(Constants.FUNDERS, Constants.CAN_UPSERT, Constants.ID_TYPE)]
    public async Task<IActionResult> UpsertFunderGuide(FunderGuideTypeDTO funderGuideTypeDTO)
    {
        ValidateModel();
        int id = await _funderService.UpsertFunderGuide(funderGuideTypeDTO);
        return Ok(id);
    }

    [HttpPost("LogoImage")]
    [AuthorizePermission(Constants.FUNDERS, Constants.CAN_ADD)]
    public async Task<IActionResult> UploadLogoImage([FromQuery] int id, IFormFile logoImage)
    {
        if (logoImage == null)
        {
            return BadRequest();
        }
        await _funderService.SaveLogoImageAsync(id, logoImage);
        return Ok();
    }

    [HttpPost("Document")]
    [AuthorizePermission(Constants.FUNDERS, Constants.CAN_ADD)]
    public async Task<IActionResult> UploadDoc([FromQuery] int id, IFormFile document)
    {
        if (document == null)
        {
            return BadRequest();
        }
        await _funderService.SaveDocumentAsync(id, document);
        return Ok();
    }

    [HttpPost("Documents")]
    [AuthorizePermission(Constants.FUNDERS, Constants.CAN_VIEW)]
    public async Task<IActionResult> UploadDoc([FromQuery] int id, BaseModelSearchEntity searchModel)
    {
        BaseResponseDTO<DocumentDTO> docuemnts = await _funderService.GetDcoumentsAsync(id, searchModel);
        return Ok(docuemnts);
    }

    [HttpGet("Document")]
    [AuthorizePermission(Constants.FUNDERS, Constants.CAN_VIEW)]
    public async Task<IActionResult> GetDocument([FromQuery] string docName)
    {
        ValidateString(docName);
        byte[] pdf = await _funderService.GetPdfDocumentAsync(docName);
        return  File(pdf, "application/pdf", docName);
    }

    [HttpDelete("Document")]
    [AuthorizePermission(Constants.FUNDERS, Constants.CAN_DELETE)]
    public async Task<IActionResult> DeleteDocument([FromQuery] int id)
    {
        ValidateId(id);
        await _funderService.DeleteDocAsync(id);
        return Ok();
    }

    [HttpDelete("Funder/{id:int}")]
    public async Task<IActionResult> DeleteFunder(int id)
    {
        ValidateId(id);
        await _funderService.DeleteFunderAsync(id);
        return Ok();
    }

}
