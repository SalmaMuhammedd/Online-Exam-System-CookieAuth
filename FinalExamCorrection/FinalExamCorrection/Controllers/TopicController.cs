using FinalExamCorrection.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamCorrection.Controllers
{
	[Authorize(Roles = "Instructor")]
	public class TopicController : Controller
	{
		private readonly FinalOnlineExamSystemContext context;

		public TopicController(FinalOnlineExamSystemContext context)
		{
			this.context = context;
		}
		public async Task<IActionResult> Index(int id)
		{
			ViewBag.CourseId = id;
			//return View(await departmentRepository.GetAllAsync("sp_SelectDepartments"));
			var AllTopic = context.Topics.FromSqlRaw($"sp_SelectTopicByCrsID {id}").AsEnumerable().ToList();

			return View(AllTopic);

		}

		public async Task<IActionResult> GetTopicById(int Id)
		{
			//return View(await departmentRepository.GetByIdAsync(Id, "sp_GetDepartmentById"));
			var Topic = context.Topics.FromSqlRaw($"sp_SelectTopicByID {Id}").AsEnumerable().FirstOrDefault();
			return View(Topic);
		}

		public async Task<IActionResult> AddNewTopic(int id)
		{

			var Course = context.Courses.Where(c => c.CourseId == id).AsEnumerable().FirstOrDefault();
			if (Course != null)
			{
				ViewBag.CrsId = id;

				return View(new Topic { CrsId = id });
			}
			return View("Error", new ErrorViewModel() { Message = "Can not find Course", RequestId = "1002" });

		}

		[HttpPost]
		public async Task<IActionResult> AddNewTopic(Topic topic)
		{
			if (ModelState.IsValid)
			{
				//var result = await departmentRepository.Add(new Department() { dept_name = department.dept_name });
				var result = await context.Database.ExecuteSqlRawAsync(
							$"EXEC sp_InsertTopic @topicName={topic.TopicName}, @crsID={topic.CrsId}");
				if (result > 0)
					return RedirectToAction("Index", new { id = topic.CrsId });
			}
			ViewBag.CourseId = topic.CrsId;
			return View(topic);
		}
		public async Task<IActionResult> UpdateTopic(int Id)
		{
			var topic = context.Topics.FromSqlRaw($"sp_SelectTopicByID {Id}").AsEnumerable().FirstOrDefault();
			if (topic != null)
			{
				return View(topic);
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateTopic(int Id, Topic topic)
		{
			if (Id != topic.TopicId) return NotFound();
			if (ModelState.IsValid)
			{

				var result = await context.Database.ExecuteSqlRawAsync(
					$"EXEC sp_UpdateTopic @topicID = {Id}, @name = {topic.TopicName}, @crsID = {topic.CrsId}");

				if (result > 0)
					return RedirectToAction("Index", new {id = topic.CrsId});
			}

			return View(topic);
		}

		public async Task<IActionResult> DeleteTopic(int Id)
		{
			var topic = context.Topics.FromSqlRaw($"sp_SelectTopicByID {Id}").AsEnumerable().FirstOrDefault();
			if (topic != null)
			{
				var result = context.Database.ExecuteSqlRaw($"sp_DeleteTopic {Id}");
				if (result > 0)
					return RedirectToAction("Index", new { id = topic.CrsId });
			}

			return View("Error", new ErrorViewModel() { Message = "Can not delete this topic", RequestId = "1003" });
		}
	}
}
