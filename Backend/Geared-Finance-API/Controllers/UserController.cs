using Entities.DTOs;
using Entities.UtilityModels;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

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
        //[HttpGet]
        public async Task<IActionResult> GetAll([FromBody] UserSearchEntity seachParams)
        {
            IEnumerable<UserDTO> userData = await _service.GetUsersAsync(seachParams);
            if (!userData.Any())
            {
                return NotFound();
            }
            return Ok(userData);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.AddUserAsync(model);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Vendors()
        {
            IEnumerable<VendorDTO> vendorData = await _service.GetAllVendors();
            if (!vendorData.Any())
            {
                return BadRequest(); 
            }
            return Ok(vendorData);
        }


    }
}
