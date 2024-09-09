
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Geared_Finance_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected void ValidateId(int Id)
    {
        if (Id <= 0)
        {
            throw new BadHttpRequestException(Constants.BAD_REQUEST);
        }
    }

    protected void ValidateModel()
    {
        if (!ModelState.IsValid)
        {
            throw new BadHttpRequestException(Constants.INVALID_MODEL);
        }
    }

    protected void ValidateString(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            throw new BadHttpRequestException(Constants.BAD_REQUEST);
        }
    }
}
