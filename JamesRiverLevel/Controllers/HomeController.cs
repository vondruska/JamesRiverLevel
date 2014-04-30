namespace JamesRiverLevel.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Xml;
    using System.Xml.Serialization;

    using Helper;

    using Models;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using ViewModel;

    public class HomeController : Controller
    {
        #if !DEBUG
        [OutputCache(Duration = 120)]
        #endif
        public ActionResult Index()
        {
            var results = NWS.GetRiverInformation();
            return View(NWS.Parse(results));
        }

    }
}
