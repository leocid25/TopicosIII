using System.Web.Mvc;

namespace ClinicaVeterinaria.Areas.Curiosos
{
    public class CuriososAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Curiosos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Curiosos_default",
                "Curiosos/{controller}/{action}/{id}",
                new { controller = "Curiosos", action = "Index", id = UrlParameter.Optional },
                new[] { "ClinicaVeterinaria.Areas.Curiosos.Controllers" }
            );
        }
    }
}