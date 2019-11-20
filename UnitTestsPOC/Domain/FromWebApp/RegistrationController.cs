using System.Web.Mvc;

namespace UnitTestsPOC.Tests.FromWebApp
{
    public class RegistrationController : Controller
    {
        //[HttpPost]
        public ActionResult ValidateSetup(RegistrationSetupVM viewModel)
        {
            return PartialView(new RegistrationSetupVM());
        }
    }
}
