using System.Web.Mvc;

namespace ClinicaVeterinaria.Areas.Veterinarios
{
    public class VeterinariosAreaRegistration : AreaRegistration 
    {
        public override string AreaName
        {
            get 
            {
                return "Veterinarios";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Veterinarios_default",
                "Veterinarios/{controller}/{action}/{id}",
                new { controller = "Veterinarios", action = "Index", id = UrlParameter.Optional },
                new[] { "ClinicaVeterinaria.Areas.Veterinarios.Controllers" }
            );
        }
    }
}