using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK.Account;
using Stormpath.SDK.Client;

namespace StormpathCustomSamlDemo.Controllers
{
    namespace StormpathExample.Controllers
    {
        // This API controller is protected and requires the user to be logged in.
        // For authorization policies, see the comments in Startup.
        [Authorize]
        public class ProfileController : Controller
        {
            private readonly IClient _stormpathClient;
            private readonly IAccount _stormpathAccount;

            public ProfileController(IClient stormpathClient, Lazy<IAccount> stormpathAccount)
            {
                // Stormpath request objects injected via DI
                _stormpathClient = stormpathClient;
                _stormpathAccount = stormpathAccount.Value;
            }

            public IActionResult Index()
            {
                return View();
            }
        }
    }
}
