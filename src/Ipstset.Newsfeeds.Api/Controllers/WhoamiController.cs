using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.Newsfeeds.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.Newsfeeds.Api.Controllers
{
    [Route("whoami")]
    [ApiController]
    public class WhoAmIController : BaseController
    {
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public ActionResult<AppUser> Get()
        {
            return this.AppUser;
        }

    }
}
