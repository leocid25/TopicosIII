using ClinicaVeterinaria.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ClinicaVeterinaria.Controllers
{
    [Authorize(Roles = "Veterinario, Secretario")]
    public class AdminController : Controller
    {
        private readonly UserService _userService;

        public AdminController()
        {
            _userService = new UserService();
        }

        [HttpGet]
        public ActionResult Register()
        {
            var roleManager = _userService.GetRoleManager();

            // Verifica o papel do usuário autenticado
            var userRole = User.IsInRole("Veterinario") ? "Veterinario" :
                           User.IsInRole("Secretario") ? "Secretario" : null;

            if (userRole == "Secretario")
            {
                // Apenas permite a opção de "Proprietário"
                ViewBag.Roles = roleManager.Roles
                    .Where(r => r.Name == "Proprietario").ToList();
            }
            else if (userRole == "Veterinario")
            {
                // Permite todas as roles menos Curioso
                ViewBag.Roles = roleManager.Roles.ToList()
                    .Where(r => r.Name != "Curioso").ToList();
            }
            else
            {
                // Não permite enxergar nenhum Role
                ViewBag.Roles = "";
            }
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userManager = _userService.GetUserManager();
                var roleManager = _userService.GetRoleManager();

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync(model.Role))
                    {
                        await roleManager.CreateAsync(new ApplicationRole(model.Role));
                    }

                    // Verifica o papel do usuário e redireciona para a ação apropriada
                    if (model.Role == "Proprietario")
                    {
                        await userManager.AddToRoleAsync(user.Id, model.Role);

                        // Redireciona para a página de criação do Proprietário
                        return RedirectToRoute(new
                        {
                            area = "Proprietarios",
                            controller = "Proprietarios",
                            action = "Create",
                            userId = user.Id
                        });
                    }
                    else if (model.Role == "Secretario")
                    {
                        await userManager.AddToRoleAsync(user.Id, model.Role);
                        // Redireciona para a página específica para Secretário
                        return RedirectToRoute(new
                        {
                            area = "Secretarios",
                            controller = "Secretarios",
                            action = "Create",
                            userId = user.Id
                        });

                    }
                    else if (model.Role == "Veterinario")
                    {
                        await userManager.AddToRoleAsync(user.Id, model.Role);
                        // Redireciona para a página específica para Veterinario
                        return RedirectToRoute(new
                        {
                            area = "Veterinarios",
                            controller = "Veterinarios",
                            action = "Create",
                            userId = user.Id
                        });
                    }

                    await userManager.AddToRoleAsync(user.Id, model.Role);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            var roles = _userService.GetRoleManager();
            ViewBag.Roles = roles.Roles.ToList();
            return View(model);
        }

        //[HttpPost]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userManager = _userService.GetUserManager();
        //        var roleManager = _userService.GetRoleManager();

        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await userManager.CreateAsync(user, model.Password);

        //        if (result.Succeeded)
        //        {
        //            if (!await roleManager.RoleExistsAsync(model.Role))
        //            {
        //                await roleManager.CreateAsync(new ApplicationRole(model.Role));
        //            }

        //            await userManager.AddToRoleAsync(user.Id, model.Role);

        //            // Gera o token de confirmação de e-mail
        //            var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            var emailConfirmationLink = Url.Action("ConfirmEmail", "Account",
        //                new { userId = user.Id, token = emailConfirmationToken }, protocol: Request.Url.Scheme);

        //            // Envia o e-mail de confirmação aqui, utilizando emailConfirmationLink
        //            await _userService.SendEmailAsync(user.Email, "Confirmação de E-mail", $"Por favor, confirme seu e-mail clicando aqui: {emailConfirmationLink}");

        //            // Redireciona para a ação apropriada baseada no papel do usuário
        //            if (model.Role == "Proprietario")
        //            {
        //                return RedirectToRoute(new { area = "Proprietarios", controller = "Proprietarios", action = "Create", userId = user.Id });
        //            }
        //            else if (model.Role == "Secretario")
        //            {
        //                return RedirectToRoute(new { area = "Secretarios", controller = "Secretarios", action = "Create", userId = user.Id });
        //            }
        //            else if (model.Role == "Veterinario")
        //            {
        //                return RedirectToRoute(new { area = "Veterinarios", controller = "Veterinarios", action = "Create", userId = user.Id });
        //            }

        //            return RedirectToAction("Index", "Home");
        //        }
        //        AddErrors(result);
        //    }

        //    var roles = _userService.GetRoleManager();
        //    ViewBag.Roles = roles.Roles.ToList();
        //    return View(model);
        //}







        public ActionResult ManageRoles()
        {
            var roleManager = _userService.GetRoleManager();
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var roleManager = _userService.GetRoleManager();
                await roleManager.CreateAsync(new ApplicationRole(roleName));
            }
            return RedirectToAction("ManageRoles");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
