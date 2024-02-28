using FinalExamCorrection.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FinalExamCorrection.Controllers
{
	public class QuestionController : Controller
	{
		private readonly FinalOnlineExamSystemContext context;
		public QuestionController(FinalOnlineExamSystemContext context)
		{
			this.context = context;
		}
		public ActionResult Index()
		{
			var model = context.Questions.Include(i => i.Crs).Include(i => i.Choices).ToList();
			return View(model);
		}


		public ActionResult Details(int id)
		{
			return View();
		}


		public ActionResult Create()
		{
			var courses = context.Courses.FromSql($"sp_SelectCourses").AsEnumerable().ToList();
			ViewBag.Courses = new SelectList(courses, "CourseId", "CourseName");
			return View();
		}


		[HttpPost]
		public ActionResult Create(Question model, List<string> choicetext, bool correct1, bool correct2, bool correct3)
		{
			context.Database.ExecuteSqlInterpolated($@"
                sp_InsertQuestion 
                {model.Text}, 
                {model.Type}, 
                {model.Score}, 
                {model.Level}, 
                {model.CrsId}
            ");

			int? qsid = context.Questions.OrderBy(i => i.QestId).LastOrDefault()?.QestId;
			int i = 0;
			context.Database.ExecuteSqlInterpolated($@"
                sp_InsertChoice 
                {choicetext[i]}, 
                {correct1}, 
                {qsid}
            ");

			i++;

			context.Database.ExecuteSqlInterpolated($@"
                sp_InsertChoice 
                {choicetext[i]}, 
                {correct2}, 
                {qsid}
            ");

			i++;

			context.Database.ExecuteSqlInterpolated($@"
                sp_InsertChoice 
                {choicetext[i]}, 
                {correct3}, 
                {qsid}
            ");

			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id)
		{
			var model = context.Questions.FromSql($"sp_SelectQuestionByID {id}").AsEnumerable().FirstOrDefault();
			return View(model);
		}

		[HttpPost]
		public ActionResult Edit(int id, Question model, List<string> choicetext, bool correct1, bool correct2, bool correct3)
		{
			context.Database.ExecuteSqlInterpolated($@"
                sp_UpdateQuestion
                {id},
                {model.Text}, 
                {model.Type}, 
                {model.Score}, 
                {model.Level} 
            ");
			var choices = context.Choices
										.FromSqlInterpolated($"sp_SelectChoiceByQuestionId {id}")
										.AsEnumerable()
										.ToList();
			int i = 0;
			context.Database.ExecuteSqlInterpolated($@"
                sp_UpdateChoice 
                {choices[i].ChoiceId},
                {choicetext[i]}, 
                {correct1}, 
                {id}
            ");

			i++;

			context.Database.ExecuteSqlInterpolated($@"
                sp_UpdateChoice 
                {choices[i].ChoiceId},
                {choicetext[i]}, 
                {correct2}, 
                {id}
            ");

			i++;

			try
			{
                context.Database.ExecuteSqlInterpolated($@"
                sp_UpdateChoice 
                {choices[i].ChoiceId},
                {choicetext[i]}, 
                {correct3}, 
                {id}
            ");
            }
			catch { }



            return RedirectToAction("Index");
		}

		public ActionResult Delete(int id)
		{
			var model = context.Questions.FromSql($"sp_SelectQuestionByID {id}").AsEnumerable().FirstOrDefault();
			var cs = context.Courses.FromSql($"sp_GetCourseById {model?.CrsId}").AsEnumerable().FirstOrDefault();
			ViewBag.Course = cs;
			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int id, Question model)
		{
			if (model != null)
			{
				var result = context.Database.ExecuteSqlInterpolated($"EXEC sp_DeleteQuestion {id}");

				if (result > 0)
					return RedirectToAction(nameof(Index));
			}

			return View("Error", new ErrorViewModel() { Message = "Can not delete this Question", RequestId = "1001" });

		}
	}
}
