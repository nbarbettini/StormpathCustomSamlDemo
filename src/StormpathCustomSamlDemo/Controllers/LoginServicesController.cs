using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK;
using Stormpath.SDK.Application;

namespace StormpathCustomSamlDemo.Controllers
{
    public class LoginServicesController : Controller
    {
        private readonly IApplication _stormpathApplication;

        public LoginServicesController(IApplication stormpathApplication)
        {
            _stormpathApplication = stormpathApplication;
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
