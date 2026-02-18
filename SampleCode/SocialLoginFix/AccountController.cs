using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShopOnWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Initiates external authentication using redirect-based OAuth flow.
        /// This replaces the problematic popup-based approach.
        /// </summary>
        /// <param name="provider">OAuth provider name (e.g., "Google", "Facebook")</param>
        /// <param name="returnUrl">URL to redirect to after successful authentication</param>
        /// <returns>ChallengeResult that redirects to OAuth provider</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Preserve the return URL for after authentication completes
            var redirectUrl = Url.Action(
                nameof(ExternalLoginCallback), 
                "Account", 
                new { ReturnUrl = returnUrl });

            // Configure authentication properties
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(
                provider, 
                redirectUrl);

            // Return a ChallengeResult that triggers a redirect to the OAuth provider
            // This is the KEY CHANGE from popup-based flow:
            // - Browser does a full page redirect (not blocked)
            // - After OAuth, provider redirects back to our callback URL
            return new ChallengeResult(provider, properties);
        }

        /// <summary>
        /// Callback endpoint for external OAuth providers.
        /// Handles the response after user authenticates with external provider.
        /// </summary>
        /// <param name="returnUrl">Preserved return URL from initial login request</param>
        /// <param name="remoteError">Error message from OAuth provider, if any</param>
        /// <returns>Redirect to return URL or login page with error message</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(
            string returnUrl = null, 
            string remoteError = null)
        {
            // Handle errors from the OAuth provider with user-friendly messages
            if (remoteError != null)
            {
                TempData["ErrorMessage"] = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            // Get the login information from the external provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["ErrorMessage"] = "Error loading external login information. Please try again.";
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if they have a login already
            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (result.Succeeded)
            {
                // Successfully authenticated - redirect to preserved return URL
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                TempData["ErrorMessage"] = "Your account has been locked. Please contact support.";
                return RedirectToAction(nameof(Login));
            }

            // If the user does not have an account, prompt them to create one
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Provider"] = info.LoginProvider;

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel 
            { 
                Email = email 
            });
        }

        /// <summary>
        /// Handles confirmation of external login when user doesn't have an existing account.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(
            ExternalLoginConfirmationViewModel model, 
            string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    TempData["ErrorMessage"] = "Error loading external login information during confirmation.";
                    return RedirectToAction(nameof(Login));
                }

                var user = new ApplicationUser 
                { 
                    UserName = model.Email, 
                    Email = model.Email 
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        /// <summary>
        /// Safely redirects to local URL, preventing open redirect vulnerabilities.
        /// </summary>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }

    // View models
    public class ExternalLoginConfirmationViewModel
    {
        public string Email { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        // Add custom user properties here
    }
}
