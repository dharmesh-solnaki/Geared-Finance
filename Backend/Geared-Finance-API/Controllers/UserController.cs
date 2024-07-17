using Entities.DTOs;
using Entities.UtilityModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace Geared_Finance_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService Service)
        {
            _service = Service;
        }

       
        [HttpPost("GetUsers")]
        //[HttpGet]
        public async Task<IActionResult> GetAll([FromBody]UserSearchEntity seachParams)
        {
            IEnumerable<UserDTO> userData = await _service.GetUsersAsync(seachParams);
            if (!userData.Any())
            {
                return NotFound();
            }
            return Ok(userData);
        }
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.AddUserAsync(model);
            return Ok();

        }


    }
}
