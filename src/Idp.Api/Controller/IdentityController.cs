using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Api.Controller
{
    [Route("api/[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
    }
}