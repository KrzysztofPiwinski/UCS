using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UCS.Db.Entities;
using UCS.Web.Models;
using UCS.Web.Models.Repositories;
using UCS.Web.ViewModels;

namespace UCS.Web.Controllers
{
    public class StudentController : AbstractController
    {
        private StudentRepository _repository = null;

        public StudentController() : base()
        {
            _repository = new StudentRepository();
        }

        public ActionResult Index(int page = 1)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.STUDENTS))
            {
                return RedirectToAction("Menu", "Home");
            }

            if (page < 1)
            {
                page = 1;
            }

            int pageCount = (int)Math.Ceiling(_repository.GetAll().Count / (double)Configuration.PageSize);

            if (pageCount > 0 && pageCount < page)
            {
                page = pageCount;
            }
            List<Student> studentsDb = _repository.GetPage(page);


            StudentsViewModel model = StudentsViewModel.FromDb(studentsDb);
            model.UserName = GetEmail();
            model.PageCount = pageCount;
            model.CurrentPage = page;
            return View(model);
        }

        public ActionResult ChangeStatus(int id, int page)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.STUDENTS))
            {
                return RedirectToAction("Menu", "Home");
            }

            Student studentDb = _repository.GetById(id);
            if(studentDb!=null)
            {
                if(studentDb.DeletedAt.HasValue)
                {
                    studentDb.DeletedAt = null;
                    Session[Configuration.Information] = $"Student {studentDb.UserName} został aktywowany.";
                }
                else
                {
                    studentDb.DeletedAt = DateTime.Now;
                    Session[Configuration.Information] = $"Student {studentDb.UserName} został dezaktywowany.";
                }
                _repository.Edit(studentDb);
            }

            return RedirectToAction("Index", new { page = page });
        }
    }
}