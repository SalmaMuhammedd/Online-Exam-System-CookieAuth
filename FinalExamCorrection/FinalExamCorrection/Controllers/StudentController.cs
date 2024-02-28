using FinalExamCorrection.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamMVC.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly FinalOnlineExamSystemContext dbContext;
        public StudentController(FinalOnlineExamSystemContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
  
        public IActionResult TakeExam(int examId)
        {
            string id = User.Identity.Name;

            var student = dbContext.Students.FromSql($"sp_GetStudentById {id}").AsEnumerable().FirstOrDefault();
            var exam = dbContext.Exams.FromSql($"sp_SelectExamByID {examId}").AsEnumerable().FirstOrDefault();
            var questions = dbContext.Questions.FromSql($"sp_SelectExamQuestionsById {examId}").ToList();
            var questionChoices = new Dictionary<int, List<Choice>>();

            foreach (var question in questions)
            {
                var choices = dbContext.Choices.FromSql($"SelectQuestionChoices {question.QestId}").ToList();
                questionChoices.Add(question.QestId, choices);

            }

            ViewBag.exam = exam;
            ViewBag.questions = questions;
            ViewBag.questionChoices = questionChoices;

            return View(student);
        }

        [HttpPost]
        public IActionResult SubmitExam(int examId, Dictionary<int, int> selectedAnswers)
        {
            string id = User.Identity.Name;

            var exam = dbContext.Exams.FromSql($"sp_SelectExamByID {examId}").AsEnumerable().FirstOrDefault();

            //Save Answers in DB
            foreach (var questionId in selectedAnswers.Keys)
            {
                try
                {
                    var answerId = selectedAnswers[questionId];
                    dbContext.Database.ExecuteSqlInterpolated($"EXEC QuestionAnswer @examID={exam.ExamId}, @studID = {id}, @questionId = {questionId}, @answerId = {answerId}");
                }
                catch { }
                
            }

            //Correct the exam and display grade
            //ExamCorrection
            //@stdid INT,
            //@examid INT
            dbContext.Database.ExecuteSqlInterpolated($"EXEC ExamCorrection @stdid = {id}, @examID={examId}");

            return RedirectToAction("GetExam", "Exam", new { id = examId });
        }
        public IActionResult Home()
        {
            string id = User.Identity.Name;
            var student = dbContext.Students.FromSql($"sp_GetStudentById {id}").AsEnumerable().FirstOrDefault();
            return View(student);
        }

        public IActionResult StudentCourses()
        {
            string id = User.Identity.Name;

            var student = dbContext.Students.FromSql($"sp_GetStudentById {id}").AsEnumerable().FirstOrDefault();
            var courses = dbContext.Courses.FromSql($"sp_SelectStudentCoursesById {id}").ToList();

            ViewBag.courses = courses;
            return View(student);
        }

        public IActionResult StudentGrades()
        {
            string id = User.Identity.Name;

            var student = dbContext.Students.FromSql($"sp_GetStudentById {id}").AsEnumerable().FirstOrDefault();

            var exams = dbContext.Exams.FromSql($"StudentGrades {id}").ToList();
            var courses = new Dictionary<int ,Course>();

            foreach(Exam exam in exams)
            {
                courses[exam.ExamId] = (dbContext.Courses.FromSql($"sp_GetCourseById {exam.CrsId}").AsEnumerable().FirstOrDefault());
            }

            ViewBag.exams = exams;
            ViewBag.courses = courses; 

            return View(student);
        }

        public IActionResult DueExams()
        {
            string id = User.Identity.Name;

            var student = dbContext.Students.FromSql($"sp_GetStudentById {id}").AsEnumerable().FirstOrDefault();
            var exams = dbContext.Exams.FromSql($"SelectNotTakenExamByStudentId {id}").ToList();
            var examCourses = new Dictionary<Exam, Course>();

            foreach (Exam exam in exams)
            {
                examCourses[exam] = (dbContext.Courses.FromSql($"sp_GetCourseById {exam.CrsId}").AsEnumerable().FirstOrDefault());
            }

            ViewBag.examCourses = examCourses;

            return View(student);
        }
    }
}
