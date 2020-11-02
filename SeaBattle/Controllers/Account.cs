using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeaBattle.Models.Forms;

namespace SeaBattle.Controllers
{
    public class Account : Controller
    {
        UserManager<IdentityUser> manager;
        SignInManager<IdentityUser> signIn;
        public Account(UserManager<IdentityUser> _manager,SignInManager<IdentityUser> _signIn)
        {
            manager = _manager;
            signIn = _signIn;
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginForm() { ReturnUrl = returnUrl});
        }
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View(new LoginForm());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoginForm form)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser(form.Username);
                var result = await manager.CreateAsync(user, form.Password);
                if (result.Succeeded)
                {
                    await signIn.PasswordSignInAsync(user, form.Password, false, false);
                    return Redirect("/");
                }
                ModelState.AddModelError(null, "Ошибка в регистрации");
            }
            return View(form);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginForm form)
        {
            if (!ModelState.IsValid) return View(form);
            var user = await manager.FindByNameAsync(form.Username);
            if (user != null)
            {
                if((await signIn.PasswordSignInAsync(user, form.Password, false, false)).Succeeded)
                {
                    return Redirect(form?.ReturnUrl ?? "/");
                }
            }
            ModelState.AddModelError(null, "Введены неправильные имя или пароль");
            return View(form);
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await signIn.SignOutAsync();
            return Redirect("/");
        }
    }
}
