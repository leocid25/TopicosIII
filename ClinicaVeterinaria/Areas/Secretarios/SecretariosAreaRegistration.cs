using System.Web.Mvc;

namespace ClinicaVeterinaria.Areas.Secretarios
{
    public class SecretariosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Secretarios";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Secretarios_default",
                "Secretarios/{controller}/{action}/{id}",
                new { controller = "Secretarios", action = "Index", id = UrlParameter.Optional },
                new[] { "ClinicaVeterinaria.Areas.Secretarios.Controllers" }
            );
        }
    }
}