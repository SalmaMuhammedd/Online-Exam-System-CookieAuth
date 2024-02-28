using FinalExamCorrection.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace FinalExamCorrection.Controllers
{
	public class UserController : Controller
	{
		private readonly FinalOnlineExamSystemContext dbcontext;

		public UserController(FinalOnlineExamSystemContext dbcontext)
		{
			this.dbcontext = dbcontext;
		}

		//Get All Students
		public IActionResult GetAllStudent()
		{
			var students = dbcontext.Students.FromSqlRaw($"sp_SelectStudents").AsEnumerable().ToList();
            for(int i=0;i<students.Count;i++)
            {
				students[0].Dept= dbcontext.Departments.FromSqlRaw($"sp_GetDepartmentById {students[0].DeptId}").AsEnumerable().FirstOrDefault();

			}
            ViewBag.Departments = dbcontext.Departments.FromSqlRaw($"sp_SelectDepartments")
													   .AsEnumerable().ToList();
			return View(students);
		}


		//Add New Student
		public IActionResult AddStudent()
		{
			ViewBag.Departments = dbcontext.Departments.FromSqlRaw($"sp_SelectDepartments")
													   .AsEnumerable().ToList();
			return View();
		}

		[HttpPost]
		public IActionResult AddStudent(Student student)
		{
			if (ModelState.IsValid)
			{
				var newId = Guid.NewGuid().ToString();
				var result = dbcontext.Database.ExecuteSqlRaw(
					"EXEC sp_InsertStudent @id, @Fname, @Lname, @DoB, @dept_id, @email, @password",
					new SqlParameter("@id", newId),
					new SqlParameter("@Fname", student.Fname),
					new SqlParameter("@Lname", student.Lname),
					new SqlParameter("@DoB", student.DoB),
					new SqlParameter("@dept_id", student.DeptId),
					new SqlParameter("@email", student.Email),
					new SqlParameter("@password", student.Password)
				);

				if (result > 0)
					return RedirectToAction("GetAllStudent");
			}

			ViewBag.Departments = dbcontext.Departments.FromSqlRaw("EXEC sp_SelectDepartments").AsEnumerable().ToList();
			return View(student);
		}
		//Student Details
		public IActionResult StudentDetails(string Id)
		{
			var student = dbcontext.Students.FromSql($"sp_GetStudentById {Id}").AsEnumerable().FirstOrDefault();
			if (student != null)
			{
				ViewBag.Department = dbcontext.Departments.FromSqlRaw($"sp_GetDepartmentById {student.DeptId}").AsEnumerable().FirstOrDefault();
				return View(student);
			}

			return View("Error", new ErrorViewModel() { Message = "No such student found in the database", RequestId = "1002" });

		}

		//Update Student
		public async Task<IActionResult> updateStudent(string Id)
		{
			var student = dbcontext.Students.FromSql($"sp_GetStudentById {Id}").AsEnumerable().FirstOrDefault();
			if (student != null)
			{
				ViewBag.Departments = dbcontext.Departments.FromSqlRaw("EXEC sp_SelectDepartments").AsEnumerable().ToList();

				return View(student);
			}
			return View("Error", new ErrorViewModel() { Message = "No such student found in the database", RequestId = "1002" });
		}



		[HttpPost]
		public IActionResult UpdateStudent(string id, Student viewModel)
		{
			if (id != viewModel.Id)
				return View("Error", new ErrorViewModel() { Message = "Error occurred while updating student's data", RequestId = "1002" });

			if (ModelState.IsValid)
			{
				var student = dbcontext.Students.FromSql($"sp_GetStudentById {id}").AsEnumerable().FirstOrDefault();
				if (student != null)
				{
					var result = dbcontext.Database.ExecuteSqlRaw(
						"EXEC sp_UpdateStudent @StudId, @StudFName, @StudLName, @DOB, @DeptId, @Email, @Password",
						new SqlParameter("@StudId", student.Id),
						new SqlParameter("@StudFName", viewModel.Fname),
						new SqlParameter("@StudLName", viewModel.Lname),
						new SqlParameter("@DOB", viewModel.DoB),
						new SqlParameter("@DeptId", viewModel.DeptId),
						new SqlParameter("@Email", student.Email),
						new SqlParameter("@Password", student.Password)
					);
					if (result > 0)
					{
						return RedirectToAction("GetAllStudent");
					}
				}
			}

			ViewBag.Departments = dbcontext.Departments.FromSqlRaw("EXEC sp_SelectDepartments").AsEnumerable().ToList();
			return View(viewModel);
		}



		//Delete Student
		public IActionResult DeleteStudent(string id)
		{
			var student = dbcontext.Students.FromSqlRaw("EXEC sp_GetStudentById @Id", new SqlParameter("@Id", id)).AsEnumerable().FirstOrDefault();
			if (student != null)
			{
				try
				{
					var result = dbcontext.Database.ExecuteSqlRaw("EXEC sp_DeleteStudent @StudId", new SqlParameter("@StudId", student.Id));
					if (result > 0)
						return RedirectToAction(nameof(GetAllStudent));
				}
				catch (Exception ex)
				{
					return View("Error", new ErrorViewModel() { Message = ex.Message, RequestId = "1002" });
				}
			}
			return View("Error", new ErrorViewModel() { Message = "No such student in the db", RequestId = "1002" });
		}


	}
}
