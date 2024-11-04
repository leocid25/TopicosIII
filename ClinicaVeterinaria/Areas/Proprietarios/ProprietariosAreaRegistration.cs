using System.Web.Mvc;

namespace ClinicaVeterinaria.Areas.Proprietarios
{
    public class ProprietariosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Proprietarios";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Proprietarios_default",
                "Proprietarios/{controller}/{action}/{id}",
                new { controller = "Proprietarios", action = "Index", id = UrlParameter.Optional },
                new[] { "ClinicaVeterinaria.Areas.Proprietarios.Controllers" }
            );
        }
    }
}