namespace JamesRiverLevel.Controllers
{
    using System;
    using System.Web.Mvc;
    using Helper;
    using ViewModel;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.Application["ViewModel"] == null
                || ((DisplayViewModel)HttpContext.Application["ViewModel"]).DataObtainedAt.AddSeconds(300)
                    < DateTime.Now)
            {
                var results = NWS.GetRiverInformation();

                var viewModel = NWS.Parse(results);

                HttpContext.Application["ViewModel"] = viewModel;
            }

            return HttpContext.Application["ViewModel"] == null ? View("Error") : View(HttpContext.Application["ViewModel"]);
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult GetChartData()
        {
            if (HttpContext.Application["ViewModel"] == null
                || ((DisplayViewModel)HttpContext.Application["ViewModel"]).DataObtainedAt.AddSeconds(300)
                    < DateTime.Now)
            {
                var results = NWS.GetRiverInformation();

                var viewModel = NWS.Parse(results);

                HttpContext.Application["ViewModel"] = viewModel;
            }

            return HttpContext.Application["ViewModel"] == null ? View("Error") : View(HttpContext.Application["ViewModel"]);
        }
    }
}
