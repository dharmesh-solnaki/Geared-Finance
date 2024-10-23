using Entities.DTOs;
using Entities.UtilityModels;
using Geared_Finance_API.Auth;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Utilities;

namespace Geared_Finance_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolePermissionController : ControllerBase
{
    private readonly IRolePermissionService _rolePermissionService;
    public RolePermissionController(IRolePermissionService rolePermissionService)
    {
        _rolePermissionService = rolePermissionService;
    }

    [HttpGet]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_VIEW)]
    public IActionResult GetModules()
    {
        IEnumerable<ModulesDTO> modules = _rolePermissionService.GetModules();
        if (!modules.Any()) return NoContent();
        return Ok(modules);
    }

    [HttpGet("Rights")]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_VIEW)]
    public async Task<IActionResult> GetRolePermissions([FromQuery] int roleId)
    {
        IEnumerable<RightsDTO> rights = await _rolePermissionService.GetRolePermissionAsync(roleId);
        if (!rights.Any()) return NoContent();
        return Ok(rights);
    }

    [HttpPost]
    [AuthorizePermission(Constants.SETTINGS, Constants.CAN_UPSERT, Constants.ID_TYPE)]
    public async Task<IActionResult> UpsertRights(IEnumerable<RightsDTO> rightsDTOs)
    {
        if (!rightsDTOs.Any()) return BadRequest();
        await _rolePermissionService.UpsertRightsAsync(rightsDTOs);
        return Ok();
    }

    [HttpPost("Roles")]
    public IActionResult GetAllRoles(BaseModelSearchEntity model)
    {
        IEnumerable<RoleDTO> roles = _rolePermissionService.GetAllRoles(model);
        if (!roles.Any()) return NoContent();
        return Ok(roles);
    }

}
