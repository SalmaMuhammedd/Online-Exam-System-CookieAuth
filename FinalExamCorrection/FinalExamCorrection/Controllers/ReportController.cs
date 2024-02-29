using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalExamCorrection.Controllers
{
    [Authorize(Roles="Instructor")]
    public class ReportController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
