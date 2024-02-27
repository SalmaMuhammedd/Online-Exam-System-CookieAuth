using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalExamCorrection.Models;

namespace ExamMVC.Controllers
{
    public class ExamController : Controller
    {
        private readonly FinalOnlineExamSystemContext dbContext;
        public ExamController(FinalOnlineExamSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult GetExam(int id)
        {
            var exam = dbContext.Exams.Where(e => e.ExamId == id).FirstOrDefault();

            ViewBag.Student = dbContext.Students.FromSql($"SelectStudentByExamId {id}").AsEnumerable().FirstOrDefault();
            ViewBag.Course = dbContext.Courses.FromSql($"SelectCourseByExamId {id}").AsEnumerable().FirstOrDefault();
            return View(exam);
        }
    }
}
