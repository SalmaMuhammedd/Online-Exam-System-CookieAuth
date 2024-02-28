using FinalExamCorrection.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FinalExamCorrection.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
        private readonly FinalOnlineExamSystemContext dbContext;

        public InstructorController(FinalOnlineExamSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Home()
        {
            string id = User.Identity.Name;
            var courses = dbContext.Courses.FromSqlInterpolated($"getCoursesByInstructor {id}").AsEnumerable().ToList();

            ViewBag.courses = courses.Select(c => new SelectListItem
            {
                Text = c.CourseName,
                Value = c.CourseId.ToString()
            }).ToList();

            var firstCourseIndex = courses.Select(c => c.CourseId).FirstOrDefault();

            ViewBag.firstExams = dbContext.Exams.FromSqlRaw($"getExamsofCourse {firstCourseIndex}").AsEnumerable().ToList();

            ViewBag.StudentsOfFirstCourse = dbContext.Students.FromSqlRaw($"getStudentInSpecificCourse {firstCourseIndex}").AsEnumerable().ToList();

            return View();
        }

        public IActionResult DeleteExam(int id)
        {
            dbContext.Database.ExecuteSqlRaw($"EXEC sp_DeleteExam {id}");
            dbContext.SaveChanges();
            return RedirectToAction("Home");
        }

        [HttpGet]
        public IActionResult EditExam(int id)
        {
            ViewBag.Exam = dbContext.Exams.FromSqlRaw($"sp_SelectExamByID {id}").AsEnumerable().FirstOrDefault();

            return View();
        }

        [HttpPost]
        public IActionResult EditExam(Exam exam)
        {
            Debug.WriteLine($"exam id   {exam.ExamId}");
            Debug.WriteLine(exam.Duration);
            int duration = exam.Duration ?? 0;
            string date = exam.Date?.ToString("yyyy-MM-dd") ?? "";

            // Execute the stored procedure with converted parameters
            dbContext.Database.ExecuteSqlRaw("EXEC sp_UpdateExam {0}, {1}, {2}", exam.ExamId, duration, date);
            dbContext.SaveChanges();
            return RedirectToAction("Home");
        }



        [HttpGet]
        public IActionResult GenerateExam()
        {
            string id = User.Identity.Name;

            var courses = dbContext.Courses.FromSqlRaw($"getCoursesByInstructor {id}").AsEnumerable().ToList();
            var coursesWithoutExams = courses.Where(c => !dbContext.Exams.Any(e => e.CrsId == c.CourseId)).ToList();

            if (coursesWithoutExams.Count() > 0)
            {
                ViewBag.courses = coursesWithoutExams.Select(c => new SelectListItem
                {
                    Text = c.CourseName,
                    Value = c.CourseId.ToString()
                }).ToList();


                var firstCourse = coursesWithoutExams.Select(c => c.CourseId).FirstOrDefault();

                ViewBag.numOfMsq = dbContext.Questions.Count(c => c.Type == "MCQ" && c.CrsId == firstCourse);
                ViewBag.numOfTF = dbContext.Questions.Count(c => c.Type == "TF" && c.CrsId == firstCourse);

                return View();
            }

            return RedirectToAction("Home");
        }

        [HttpPost]
        public IActionResult GenerateExam(IFormCollection formCollection)
        {
            var courseId = int.Parse(formCollection["course"]);
            var numOfTF = int.Parse(formCollection["trueFalseNum"]);
            var numOfMcq = int.Parse(formCollection["MCQNum"]);

            var students = dbContext.Students.FromSqlRaw($"getStudentInSpecificCourse {courseId} ").AsEnumerable().ToList();

            foreach (var student in students)
            {
                string studId = student.Id;
                //@studId int, @courseID int, @trueFalseNo INT, @MCQNo INT 

                dbContext.Database.ExecuteSqlInterpolated($"EXEC ExamGeneration @studId = {studId}, @CourseID = {courseId}, @trueFalseNo = {numOfTF}, @MCQNO = {numOfMcq}");
            }

            dbContext.SaveChanges();
            return RedirectToAction("Home");
        }




        [HttpGet]

        public IActionResult GetStudentsByCourse(int courseId)
        {
            var students = dbContext.Students.FromSqlRaw($"getStudentInSpecificCourse {courseId}").AsEnumerable().ToList();

            var std = students.Select(s => new SelectListItem
            {
                Text = s.Fname + " " + s.Lname,
                Value = s.Id.ToString()
            }).ToList();




            // Return the list of students as JSON
            return Json(std);
        }



        [HttpGet]
        public IActionResult GetNumberOfMSQ(int courseId)
        {
            var numOfMCQ = dbContext.Questions.Count(c => c.Type == "MCQ" && c.CrsId == courseId);
            return Json(numOfMCQ);
        }


        [HttpGet]
        public IActionResult GetNumberOfTF(int courseId)
        {
            var numOfTF = dbContext.Questions.Count(c => c.Type == "TF" && c.CrsId == courseId);
            return Json(numOfTF);
        }


        public IActionResult GetStudentsExam(int courseId)
        {
            var examsWithStudents = dbContext.Exams
                .Where(e => e.CrsId == courseId) // Filter exams by courseId
                .Join(dbContext.Students, // Join with Students
                    exam => exam.StudId, // Foreign key in Exams
                    student => student.Id, // Primary key in Students
                    (exam, student) => new // Select properties to include in the result
                    {
                        ExamId = exam.ExamId,
                        TotalMarks = exam.TotalMarks,
                        Score = exam.Score,
                        StudentName = student.Fname + " " + student.Lname // Assuming Fname and Lname are the student's first and last name
                    })
                .ToList(); // Execute the query and convert to list

            return Json(examsWithStudents);
        }





    }
}

