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
        public async Task<IActionResult> GetAll(UserSearchEntity seachParams)
        {
            BaseRepsonseDTO<UserDTO> userData = await _service.GetUsersAsync(seachParams);
            if (!userData.responseData.Any())
            {
                return NoContent();
            }
            return Ok(userData);
        }

        [HttpPost("UpsertUser")]
        public async Task<IActionResult> AddUser(UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Constants.BAD_REQUEST);
            }
            IsExistData isExistData = await _service.AddUserAsync(model);
            return Ok(isExistData);
        }


        [HttpPut]
        public async Task<IActionResult> EditUser(UserDTO model)
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


        [HttpGet("CheckValidity")]
        public async Task<IActionResult> CheckEmailAndPass(string email, string mobile)
        {
            IsExistData response = await _service.CheckValidityAsync(email, mobile);
            return Ok(response);

        }
        [HttpGet("GetRelationShipManager")]
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
        public async Task<IActionResult> DeleteUser([FromQuery] int id)
        {
            if (id <= 0) return BadRequest();
            bool isDeleted = await _service.DeleteUser(id);
            if (!isDeleted) { return NotFound(Constants.RECORD_NOT_FOUND); }
            else { return Ok(); }
        }

        [HttpGet("GetReportingTo")]
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
}
