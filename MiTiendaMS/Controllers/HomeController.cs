using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiTiendaMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var cookies = this.HttpContext.Request.Cookies;
            return View();
        }

        //[Authorize(Policy = "my-policy-1")]
        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var access_token = await this.HttpContext.GetTokenAsync("access_token");
            var id_token = await this.HttpContext.GetTokenAsync("id_token");

            ViewData["Claims"] = User.Claims;

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(access_token);
            var token = jsonToken as JwtSecurityToken;

            ViewData["ClaimsToken"] = token.Claims;

            return View();
        }
        public RedirectResult Swagger()
        {
            return Redirect("http://localhost:6107/swagger/index.html");
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
