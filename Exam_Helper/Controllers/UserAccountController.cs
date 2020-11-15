using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Exam_Helper.ViewsModel.Account;
namespace Exam_Helper.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserAccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult> Registration(UserRegistration user)
        {
            if (ModelState.IsValid)
            {
                User new_user = new User()
                {
                    UserName = user.UserName,
                    Email=user.Login,
                };
                
                var res = await _userManager.CreateAsync(new_user, user.Password);
                if (res.Succeeded)
                {
                    await _signInManager.SignInAsync(new_user, false);
                    return RedirectToAction("Index", "UserLibrary");
                }
                 
                else
                {

                   
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin user)
        {
            if (ModelState.IsValid)
            {
                var _user = await _userManager.FindByEmailAsync(user.Email);
                if (_user != null)
                {
                    var res = await _signInManager.PasswordSignInAsync(_user, user.Password, false, false);
                    
                    if (res.Succeeded)
                    {
                        return RedirectToAction("Index", "PublicLibrary");
                    }
                    else
                    { 
                        ModelState.AddModelError(string.Empty,"incorrect email or password");
                        
                    }

                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
