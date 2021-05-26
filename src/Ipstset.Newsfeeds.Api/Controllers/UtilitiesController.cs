using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.Newsfeeds.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.Newsfeeds.Api.Controllers
{
    [Route("utilities")]
    [ApiController]
    public class UtilitiesController : BaseController
    {
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Route("saltedhash")]
        //[HttpGet]
        //public ActionResult<string> GetSaltedHash(string value, string salt)
        //{
        //    if (!AppUser.HasRole("Admin"))
        //        return Unauthorized();

        //    return SaltedHash.Create(value, salt, Constants.HashKey).Hash;
        //}

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("saltedhash")]
        [HttpGet]
        public ActionResult<SaltedHash> GetSaltedHash(string value, string salt)
        {
            if (!AppUser.HasRole("Admin"))
                return Unauthorized();

            return SaltedHash.Create(value, salt, Constants.HashKey);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("salt")]
        [HttpGet]
        public ActionResult<string> GetSalt()
        {
            if (!AppUser.HasRole("Admin"))
                return Unauthorized();

            return new Salt().Value;
        }
    }
}
