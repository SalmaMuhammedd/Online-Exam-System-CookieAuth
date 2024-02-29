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

        public ActionResult GetStudentsInDepartment()
        {
            string reportUrl = "http://desktop-lgl6osr/Reports/report/DepartmentStudents";

            ViewBag.ReportUrl = reportUrl;

            return View();
        }

        public ActionResult StudentGrade()
        {
            string reportUrl = "http://desktop-lgl6osr/Reports/report/StudentGrade";

            ViewBag.ReportUrl = reportUrl;

            return View();
        }

        public ActionResult CourseTopics()
        {
            string reportUrl = "http://desktop-lgl6osr/Reports/report/CourseTopics";

            ViewBag.ReportUrl = reportUrl;

            return View();
        }

        public ActionResult ExamQuestionsAndAnswers()
        {
            string reportUrl = "http://desktop-lgl6osr/Reports/report/ExamQuestionsAndAnswers";

            ViewBag.ReportUrl = reportUrl;

            return View();
        }

        public ActionResult InstructorCourses()
        {
            string reportUrl = "http://desktop-lgl6osr/Reports/report/InstructorCourses";

            ViewBag.ReportUrl = reportUrl;

            return View();
        }

        public ActionResult StudentAnswers()
        {
            string reportUrl = "http://desktop-lgl6osr/Reports/report/StudentAnswers";

            ViewBag.ReportUrl = reportUrl;

            return View();
        }

    }
}
