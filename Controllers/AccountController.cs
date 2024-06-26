using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Contexts;
using Portfolio.Controllers;
using Portfolio.Models;

namespace MyApp.Namespace
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        // GET: AccountController
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginVM vm, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    vm.Email, vm.Password, vm.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return View(vm);
        }

        public ActionResult Register()
        {
            return View();
        }

        private async Task<IdentityResult> CreateRole(string roleName)
        {
            // We just need to specify a unique role name to create a new role
            ApplicationRole role = new ApplicationRole
            {
                Name = roleName
            };

            // Saves the role in the underlying AspNetRoles table
            IdentityResult result = await roleManager.CreateAsync(role);

            return result;
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterVM vm)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new ApplicationUser
                {
                    UserName = vm.Email,
                    Email = vm.Email
                };

                var roleToCreate = "Admin";

                if (!await roleManager.RoleExistsAsync(roleToCreate))
                {
                    var createRole = await CreateRole(roleToCreate);
                    if (!createRole.Succeeded)
                    {
                        foreach (var error in createRole.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                        return View(vm);
                    }
                }
                

                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, vm.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    if (!await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        var addRoleResult = await userManager.AddToRoleAsync(user, "Admin");
                        if (!addRoleResult.Succeeded)
                        {
                            foreach (var error in addRoleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }

                            return View(vm);
                        }
                    }

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(vm);
        }

        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
