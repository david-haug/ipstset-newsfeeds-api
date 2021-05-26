using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.Newsfeeds.Api.Auth;
using Ipstset.Newsfeeds.Application;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.Newsfeeds.Api
{
    public class BaseController : Controller
    {
        public AppUser AppUser => ClaimsService.CreateAppUser(User.Claims);
    }
}
