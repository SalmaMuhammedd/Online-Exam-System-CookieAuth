using FinalExamCorrection.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

namespace FinalExamCorrection.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class CourseController : Controller
    {
        private readonly FinalOnlineExamSystemContext _context;
        public CourseController(FinalOnlineExamSystemContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var courses = _context.Courses.Include(i => i.Dept).Include(i => i.Ins).ToList();

            return View(courses);
        }
        public IActionResult Create()
        {
            var Departments = _context.Departments.FromSql($"sp_SelectDepartments").AsEnumerable().ToList();

            ViewBag.departments = new SelectList(Departments, "DeptId", "DeptName");

            var Instructors = _context.Instructors.FromSql($"sp_SelectInstructor ").AsEnumerable().ToList();

            ViewBag.instructors = new SelectList(Instructors, "Id", "InsName");

            return View();
        }
        [HttpPost]
        public IActionResult Create(Course model)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC sp_InsertNewCourse {model.CourseName}, {model.InsId}, {model.DeptId}");
            return RedirectToAction("Index");

        }
        public IActionResult Edit(int id)
        {
            Course? course = _context.Courses.FromSql($"sp_GetCourseById {id}").AsEnumerable().FirstOrDefault();
            var Departments = _context.Departments.FromSql($"sp_SelectDepartments").AsEnumerable().ToList();

            ViewBag.departments = new SelectList(Departments, "DeptId", "DeptName");

            var Instructors = _context.Instructors.FromSql($"sp_SelectInstructor ").AsEnumerable().ToList();

            ViewBag.instructors = new SelectList(Instructors, "Id", "InsName");

            return View(course);
        }
        [HttpPost]
        public IActionResult Edit(int id, Course model)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC sp_UpdateCourse {id}, {model.CourseName}, {model.InsId}, {model.DeptId}");
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public IActionResult Delete(int id)
        {
            Course? course = _context.Courses.Where(c => c.CourseId == id).Include(i => i.Dept).Include(i => i.Ins).FirstOrDefault();
            return View(course);
        }

        [HttpPost]
        public IActionResult Delete(Course model, int id)
        {
            Course? course = _context.Courses.FromSql($"sp_GetCourseById {id}").AsEnumerable().FirstOrDefault();
            if (course != null)
            {
                var result = _context.Database.ExecuteSqlInterpolated($"EXEC sp_DeleteCourse {id}");

                if (result > 0)
                    return RedirectToAction(nameof(Index));
            }

            return View("Error", new ErrorViewModel() { Message = "Can not delete this department", RequestId = "1001" });


        }
    }
}

