using FinalExamCorrection.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamCorrection.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class DepartmentController : Controller
    {
        private readonly FinalOnlineExamSystemContext context;

        public DepartmentController(FinalOnlineExamSystemContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            //return View(await departmentRepository.GetAllAsync("sp_SelectDepartments"));
            var AllDepts = context.Departments.FromSqlRaw($"sp_SelectDepartments").AsEnumerable().ToList();

            return View(AllDepts);

        }

        public async Task<IActionResult> GetDepartmentById(int Id)
        {
            //return View(await departmentRepository.GetByIdAsync(Id, "sp_GetDepartmentById"));
            var Dept = context.Departments.FromSqlRaw($"sp_GetDepartmentById {Id}").AsEnumerable().FirstOrDefault();
            return View(Dept);
        }

        public async Task<IActionResult> AddNewDepartment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                //var result = await departmentRepository.Add(new Department() { dept_name = department.dept_name });
                var result = await context.Database.ExecuteSqlRawAsync(
                            $"EXEC sp_InsertNewDepartment {department.DeptName}");
                if (result > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        public async Task<IActionResult> UpdateDepartment(int Id)
        {
            var Dept = context.Departments.FromSqlRaw($"sp_GetDepartmentById {Id}").AsEnumerable().FirstOrDefault();
            if (Dept != null)
            {
                return View(Dept);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(int Id, Department department)
        {
            if (Id != department.DeptId) return NotFound();
            if (ModelState.IsValid)
            {
                var result = await context.Database.ExecuteSqlRawAsync(
                                $"EXEC sp_UpdateDepartment @DeptId = {department.DeptId}, @NewDeptName = {department.DeptName}");

                if (result > 0)
                    return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        public async Task<IActionResult> DeleteDepartment(int Id)
        {
            var dept = context.Departments.FromSqlRaw($"sp_GetDepartmentById {Id}").AsEnumerable().FirstOrDefault();
            if (dept != null)
            {
                var result = context.Database.ExecuteSqlRaw($"sp_DeleteDepartment {Id}");
                if (result > 0)
                    return RedirectToAction(nameof(Index));
            }
            
            return View("Error", new ErrorViewModel(){Message="Can not delete this department", RequestId = "1001"});
        }
    }
}
