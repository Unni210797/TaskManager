using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task_Management_System.ViewModel;

namespace Task_Management_System.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        public AccountController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager; 
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {

            if (ModelState.IsValid)
            {
                var result= await signInManager.PasswordSignInAsync(loginVM.Username!, loginVM.Password!,loginVM.RememberMe,false);
                if (result.Succeeded)
                {
                  
                    return RedirectToAction("Index", "Task");
                }
                else
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(loginVM);
            }
            return View(loginVM);
        }
        public IActionResult Register()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new()
                {
                    UserName = registerVM.Name,
                    Email = registerVM.Email

                };
                var result= await userManager.CreateAsync(user,registerVM.Password!);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerVM);

        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
