using Entities.DTOs;
using Entities.UtilityModels;
using Microsoft.AspNetCore.Mvc;

using Service.Interface;

namespace Geared_Finance_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(BaseModelSearchEntity searchEntity)
        {
            IEnumerable<VendorDTO> vendorData = await _vendorService.GetAllVendors(searchEntity);
            if (!vendorData.Any())
            {
                return NoContent();
            }
            return Ok(vendorData);

        }

        [HttpGet("GetManagerLevels")]
        public async Task<IActionResult> ManagerLevels([FromQuery] int id)
        {

            IEnumerable<ManagerLevelDTO> managerLevelData = await _vendorService.GetManagerLevels(id);
            if (!managerLevelData.Any())
            {
                return NoContent();
            }
            return Ok(managerLevelData);
        }


    }
}
