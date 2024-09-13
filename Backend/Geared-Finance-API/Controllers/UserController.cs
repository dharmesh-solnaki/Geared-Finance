using Entities.DTOs;
using Entities.UtilityModels;
using Geared_Finance_API.Auth;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Utilities;

namespace Geared_Finance_API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UserController : BaseController
{
    private readonly IUserService _service;
    public UserController(IUserService Service)
    {
        _service = Service;
    }

    [HttpPost("Users")]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_VIEW)] 
    public async Task<IActionResult> GetAll(UserSearchEntity seachParams)
    {
        BaseResponseDTO<UserDTO> userData = await _service.GetUsersAsync(seachParams);
        if (!userData.ResponseData.Any())
        {
            return NoContent();
        }
        return Ok(userData);
    }

    [HttpPost("User")]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_UPSERT, Constants.ID_TYPE)]
    public async Task<IActionResult> UpsertUser(UserDTO model)
    {
        ValidateModel();
        IsExistData isExistData = await _service.UpsertUserAsync(model);
        return Ok(isExistData);
    }

    [HttpGet("RelationShipManagers")]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_VIEW)]
    public async Task<IActionResult> GetRelationShipManager()
    {
        IEnumerable<RelationshipManagerDTO> managerData = await _service.GetAllRelationshipManagers();
        if (!managerData.Any())
        {
            return NoContent();
        }
        return Ok(managerData);
    }

    [HttpDelete]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_DELETE)]
    public async Task<IActionResult> DeleteUser([FromQuery] int id)
    {
        //if (id <= 0) return BadRequest();
        ValidateId(id);
        bool isDeleted = await _service.DeleteUser(id);
        if (!isDeleted) { return NotFound(Constants.RECORD_NOT_FOUND); }
        else { return Ok(); }
    }

    [HttpGet("ReportingTo")]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_VIEW)]
    public async Task<IActionResult> GetReportingTo([FromQuery] int vendorId, [FromQuery] int managerLevelId = 0)
    {
        IEnumerable<RelationshipManagerDTO> reportingToList = await _service.GetReportingToListAsync(vendorId, managerLevelId);
        if (!reportingToList.Any())
        {
            return NoContent();
        }
        return Ok(reportingToList);
    }


}

