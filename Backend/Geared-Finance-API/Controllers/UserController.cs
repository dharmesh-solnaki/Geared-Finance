using Entities.DTOs;
using Entities.UtilityModels;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Utilities;

namespace Geared_Finance_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _service;


        public UserController(IUserService Service)
        {
            _service = Service;
        }


        [HttpPost("GetUsers")] 
        public async Task<IActionResult> GetAll([FromBody] UserSearchEntity seachParams)
        {
            IEnumerable<UserDTO> userData = await _service.GetUsersAsync(seachParams);
            if (!userData.Any())
            {
                return NotFound(Constants.RECORD_NOT_FOUND);
            }
            return Ok(userData);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Constants.BAD_REQUEST);
            }
            await _service.AddUserAsync(model);
            return Ok();
        }
    

        [HttpPut]
        public async Task<IActionResult> Users([FromBody] UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Constants.BAD_REQUEST);
            }
            if (model.id <= 0)
            {
               return NotFound(Constants.RECORD_NOT_FOUND);
            }
            await _service.UpdateUserAsync(model);
            return Ok();
        }

        [HttpGet("GetRelationShipManager")]
        public async Task<IActionResult> GetRelationShipManager()
        {
            IEnumerable<RelationshipManagerDTO> managerData = await _service.GetAllRelationshipManagers();
            if (!managerData.Any())
            {
               return NotFound(Constants.RECORD_NOT_FOUND);
            }
            return Ok(managerData);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (id <= 0) return BadRequest();
            bool isDeleted = await _service.DeleteUser(id);
            if (!isDeleted) {return NotFound(Constants.RECORD_NOT_FOUND); }
            else { return Ok(); }
        }

        [HttpGet("GetReportingTo")]
        public async Task<IActionResult> GetReportingTo([FromQuery] int vendorId, [FromQuery] int managerLevelId=0)
        {
          
            IEnumerable<RelationshipManagerDTO> reportingToList = await _service.GetReportingToListAsync(vendorId, managerLevelId);
            if (!reportingToList.Any())
            {
               return NotFound(Constants.RECORD_NOT_FOUND);
            }
            return Ok(reportingToList);
        }


    }
}
