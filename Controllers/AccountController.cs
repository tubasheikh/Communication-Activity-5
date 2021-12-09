using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//Make sure you have the following imports
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Lab5_1_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using System.Net.Mail;


namespace Lab5_1_.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;

        // COntructor for this controller 
        public AccountController(
                    UserManager<AppUser> userManager,
                    SignInManager<AppUser> signInManager,
                    ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }



        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion

        // httpget = when the page first loads
        [HttpGet]
        // Allow anonymus means anyone can view this page and that it doesnt require authorization
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            // if the data entered is correct
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Email, Email = model.Email };
                // wait for the user manager to verify everything is okay :)
                // user manager is a service from the identity framework 
                var result = await _userManager.CreateAsync(user, model.Password);
                // If the result is sucessful i.e. everything is in the database 
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                // If there are any errors display them using the helper functions
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            // if the data entererd is correct 
            if (ModelState.IsValid)
            {
                // store the user associated with the email 
                var user = await _userManager.FindByEmailAsync(model.Email);
                //&& await _userManager.IsEmailConfirmedAsync(user)
                if (user != null )
                {
                    // Generate and store a password reset token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    // Generate a URL 
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, protocol: Request.Scheme);

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("booksandbeyond22@gmail.com");
                        // the mail is supposed to be sent to the email the user entered 
                        mailMessage.To.Add(new MailAddress(model.Email));
                        mailMessage.Subject = "Password reset link";
                        // Add a clickable link to the mail body 
                        mailMessage.Body = "<p>Please click this <a href='" + passwordResetLink + "' >link</a> to update your password.</p>";
                        // Set isbodyhtml to true because message body has html  
                        mailMessage.IsBodyHtml = true;

                        // use teh smtpclient to send the email
                        using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                        {
                            // Enter the login credentails for the email ure using to send the emal 
                            client.Credentials = new System.Net.NetworkCredential("booksandbeyond22@gmail.com", "saeedajmal");
                            client.EnableSsl = true;
                            // send the message 
                            client.Send(mailMessage);
                        }
                    }
                    // return the view that confirms the request
                    return View("ForgotPasswordConfirmation");
                }
                // if the email is not found in the database return the email not found view
                else { return View("EmailNotFound"); }
                //return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if(token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid Password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            // if correct info is enetered 
            if (ModelState.IsValid)
            {
                // find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                // if a user is found
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    // If the token, password or the user is invalid the result will not be valid and
                    // an error message will be displayed 
                    // try tempering with the token and try clicking update 
                    // an invalid token error will be diaplayed 
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }

                    // display the error for every invalid value 
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                // if the email is tempered with in the link and update password is pressed
                // it means the email is not found in the database and return the email not found view
                else { return View("EmailNotFound"); }
                //return View("ForgotPasswordConfirmation");  
            }
            return View(model);
        }

    }
}
