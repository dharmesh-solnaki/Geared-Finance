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
    public class ApplicationController : BaseController
    {
        private readonly IApplicationService _applicationService;
        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("SalesRep")]
        [AuthorizePermission(Constants.APPLICATION_TAB, Constants.CAN_VIEW)]
        public async Task<IActionResult> GetAllSaleRepList()
        {
            IEnumerable<IdNameDTO> ownerList = await _applicationService.GetSalesRepListAsync();
            return Ok(ownerList);
        }

        [HttpPost("Leads")]
        [AuthorizePermission(Constants.APPLICATION_TAB, Constants.CAN_VIEW)]
        public async Task<IActionResult> GetAllLeads(FilterearchPayload<SqlSearchParams> payload)
        {
            BaseResponseDTO<DisplayLeadDTO> leadsRes = await _applicationService.GetAllLeadsAsync(payload);
            return Ok(leadsRes);
        }

        [HttpGet("VendorRep")]
        [AuthorizePermission(Constants.APPLICATION_TAB, Constants.CAN_VIEW)]
        public async Task<IActionResult> GetVendorRep(int id)
        {
            IEnumerable<IdNameDTO> leads = await _applicationService.GetVendorRepAsync(id);
            return Ok(leads);
        }

        [HttpGet("GetUserStatus")]
        public async Task<IActionResult> GetUserStatus()
        {
            UserStatusDTO userStatus = await _applicationService.GetUserStatus();
           return Ok(userStatus);
        }

    }
}
