using Entities.DTOs;
using Entities.UtilityModels;
using Geared_Finance_API.Auth;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Utilities;

namespace Geared_Finance_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermisionController : ControllerBase
    {
        private readonly IRolePermisionService _rolePermisionService;
        public RolePermisionController(IRolePermisionService rolePermisionService)
        {
            _rolePermisionService = rolePermisionService;
        }

        [HttpGet]
        [AuthorizePermission(ModuleConstants.SETTINGS, PermissionConstants.CAN_VIEW)]
        public async Task<IActionResult> GetModules()
        {
            IEnumerable<ModulesDTO> modules = await _rolePermisionService.GetModulesAsync();
            if (!modules.Any()) return NoContent();
            return Ok(modules);
        }
        [HttpGet("GetRights")]
        [AuthorizePermission(ModuleConstants.SETTINGS, PermissionConstants.CAN_VIEW)]
        public async Task<IActionResult> GetRolePermissions([FromQuery] int roleId)
        {
            IEnumerable<RightsDTO> rights = await _rolePermisionService.GetRolePermissionAsync(roleId);
            if (!rights.Any()) return NoContent();
            return Ok(rights);
        }

        [HttpPost]
        [AuthorizePermission(ModuleConstants.SETTINGS, PermissionConstants.CAN_UPSERT , "Id")]
        public async Task<IActionResult> UpsertRights(IEnumerable<RightsDTO> rightsDTOs)
        {
            if (!rightsDTOs.Any()) return BadRequest();
            await _rolePermisionService.UpsertRightsAsync(rightsDTOs);
            return Ok();
        }

        [HttpPost("getRoles")]
        public async Task<IActionResult> getAllRoles(BaseModelSearchEntity model)
        {
            IEnumerable<RoleDTO> roles = await _rolePermisionService.GetAllRolesAsync(model);
            if (!roles.Any()) return NoContent();
            return Ok(roles);
        }

    }
}
