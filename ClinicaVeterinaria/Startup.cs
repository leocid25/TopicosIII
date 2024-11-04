using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;
using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(ClinicaVeterinaria.Startup))]
namespace ClinicaVeterinaria
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }
        private void CreateRolesAndUsers()
        {
            using (var context = new ClinicaVeterinariaContext())
            {
                var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Criar roles Veterinario, Secretario e Proprietario se não existirem
                if (!roleManager.RoleExists("Veterinario"))
                {
                    var role = new ApplicationRole();
                    role.Name = "Veterinario";
                    roleManager.Create(role);

                    var roleS = new ApplicationRole();
                    roleS.Name = "Secretario";
                    roleManager.Create(roleS);

                    var roleP = new ApplicationRole();
                    roleP.Name = "Proprietario";
                    roleManager.Create(roleP);

                    var roleC = new ApplicationRole();
                    roleC.Name = "Curioso";
                    roleManager.Create(roleC);

                    // Criar usuário Veterinario (Admin)
                    var user = new ApplicationUser();
                    user.UserName = "vet@clinvet.com";
                    user.Email = "vet@clinvet.com";
                    string userPWD = "Senha@123";

                    var chkUser = userManager.Create(user, userPWD);

                    if (chkUser.Succeeded)
                    {
                        userManager.AddToRole(user.Id, "Veterinario");
                    }
                }
            }
        }
    }
}
