using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public async Task<IActionResult> GetAccountType(string email, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(null);
            }

            var account = await _stormpathApplication.GetAccounts()
                .Where(a => a.Email == email)
                .SingleOrDefaultAsync(ct);

            if (account == null)
            {
                return Json(null);
            }

            var directory = await account.GetDirectoryAsync(ct);
            var provider = await directory.GetProviderAsync(ct);

            return Json(new {type = provider.ProviderId.ToLower()});
        }

        [Route("[controller]/samlRedirect")]
        public async Task<IActionResult> GetSamlRedirect(string email, string next, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            var account = await _stormpathApplication.GetAccounts()
                .Where(a => a.Email == email)
                .SingleOrDefaultAsync(ct);

            if (account == null)
            {
                return BadRequest();
            }

            var directory = await account.GetDirectoryAsync(ct);

            var samlUrlBuilder = await _stormpathApplication.NewSamlIdpUrlBuilderAsync(ct);
            var redirectUrl = samlUrlBuilder
                .SetCallbackUri("http://localhost:5000/stormpathCallback")
                .SetAccountStore(directory.Href);

            if (!string.IsNullOrEmpty(next))
            {
                redirectUrl.SetState(next); // Can save a relative path in the state, then redirect after the callback
            }

            HttpContext.Response.Headers.Add("Cache-control", "no-cache, no-store");
            HttpContext.Response.Headers.Add("Pragma", "no-cache");
            HttpContext.Response.Headers.Add("Expires", "-1");

            return Redirect(redirectUrl.Build());
        }
    }
}
