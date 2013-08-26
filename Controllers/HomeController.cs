namespace JamesRiverLevel.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Xml;
    using System.Xml.Serialization;

    using Helper;

    using Models;

    using ViewModel;

    public class HomeController : Controller
    {
        [OutputCache(Duration = 120)]
        public ActionResult Index()
        {
            var results = NWS.GetRiverInformation();
            return View(NWS.Parse(results));
        }

    }
}
