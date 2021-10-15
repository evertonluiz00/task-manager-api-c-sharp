using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Api
{
    [Authorize]
    public class BaseController : ControllerBase
    {

    }
}
