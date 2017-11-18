using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UCS.Db;
using UCS.Db.Entities;
using UCS.Web.Models;
using UCS.Web.Models.Repositories;
using UCS.Web.ViewModels;

namespace UCS.Web.Controllers
{
    public class CategoryController : AbstractController
    {
        private CategoryRepository _categoryRepository = null;
        private StudentRepository _studentRepository = null;


        public CategoryController() : base()
        {
            _categoryRepository = new CategoryRepository();
            _studentRepository = new StudentRepository();
        }

        public ActionResult Index()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.CATEGORIES))
            {
                return RedirectToAction("Menu", "Home");
            }

            List<Category> categoriesDb = _categoryRepository.GetAll();

            CategoriesViewModel model = CategoriesViewModel.FromDb(categoriesDb);
            model.UserName = GetEmail();
            return View(model);
        }

        public ActionResult Add()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.CATEGORIES))
            {
                return RedirectToAction("Menu", "Home");
            }

            CategoryFormViewModel model = new CategoryFormViewModel();
            model.UserName = GetEmail();
            return View("_Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CategoryFormViewModel model)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.CATEGORIES))
            {
                return RedirectToAction("Menu", "Home");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("", "Należy podać nazwę kategorii.");
            }

            if (ModelState.IsValid)
            {
                Category categoryDb = new Category()
                {
                    Name = model.Name,
                    AddedAt = DateTime.Now
                };
                _categoryRepository.Add(categoryDb);

                Session[Configuration.Information] = "Kategoria została dodana";
                return RedirectToAction("Index");
            }

            model.HasError = true;
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.CATEGORIES))
            {
                return RedirectToAction("Menu", "Home");
            }

            Category categoryDb = _categoryRepository.GetById(id);
            if (categoryDb == null)
            {
                return RedirectToAction("Index");
            }

            CategoryFormViewModel model = CategoryFormViewModel.FromDb(categoryDb);
            model.ActionType = ActionTypeEnum.EDIT;
            model.UserName = GetEmail();
            return View("_Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryFormViewModel model)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.CATEGORIES))
            {
                return RedirectToAction("Menu", "Home");
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("", "Należy podać nazwę kategorii.");
            }

            if (ModelState.IsValid)
            {
                Category categoryDb = _categoryRepository.GetById(id);
                if (categoryDb == null)
                {
                    return RedirectToAction("Index");
                }

                categoryDb.Name = model.Name;
                _categoryRepository.Edit(categoryDb);

                return RedirectToAction("Index");
            }

            model.HasError = true;
            model.UserName = GetEmail();
            return View("_Form", model);
        }

        public ActionResult Choose()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_isStudent)
            {
                return RedirectToAction("Menu", "Home");
            }

            CategoryChooseFormViewModel model = new CategoryChooseFormViewModel();
            model.AllCategory = _categoryRepository.GetAll().Select(c => c.Name).ToList();
            model.UserCategory = _student.Categories.Select(c => c.Category.Name).ToList();
            model.UserName = GetEmail();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Choose(FormCollection formCollection)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_isStudent)
            {
                return RedirectToAction("Menu", "Home");
            }

            Student studentDb = _studentRepository.GetById(_student.Id);

            _studentRepository.RemoveCategory(studentDb);
            studentDb.Categories = new List<StudentCategory>();

            List<string> allCategory = _categoryRepository.GetAll().Select(c => c.Name).ToList();

            foreach (string category in allCategory)
            {
                string c = formCollection[category];
                if (c.StartsWith("true"))
                {
                    StudentCategory studentCategoryDb = new StudentCategory()
                    {
                        CategoryId = _categoryRepository.GetByName(category).Id,
                        StudentId = _student.Id
                    };
                    studentDb.Categories.Add(studentCategoryDb);
                }
            }
            _studentRepository.AddCategory(studentDb);


            return RedirectToAction("Menu", "Home");
        }
    }
}