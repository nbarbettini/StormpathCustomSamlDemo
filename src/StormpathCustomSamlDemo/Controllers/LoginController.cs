using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK;
using Stormpath.SDK.Application;

namespace StormpathCustomSamlDemo.Controllers
{
    public class LoginController : Controller
    {
        private readonly IApplication _stormpathApplication;

        public LoginController(IApplication stormpathApplication)
        {
            _stormpathApplication = stormpathApplication;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Route("[controller]/getAccountType")]
        public async Task<IActionResult> GetAccountType(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(null);
            }

            var account = await _stormpathApplication.GetAccounts().Where(a => a.Email == email).SingleOrDefaultAsync();

            if (account == null)
            {
                return Json(null);
            }

            return Json(new {href = account.Href});
        }
    }
}
