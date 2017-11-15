using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UCS.Db.Entities;
using UCS.Web.Models;
using UCS.Web.Models.Repositories;
using UCS.Web.ViewModels;

namespace UCS.Web.Controllers
{
    public class UserController : AbstractController
    {
        private UserRepository _repository = null;

        public UserController() : base()
        {
            _repository = new UserRepository();
        }

        public ActionResult Index()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.USERS))
            {
                return RedirectToAction("Menu", "Home");
            }

            List<User> usersDb = _repository.GetAll();

            UsersViewModel model = UsersViewModel.FromDb(usersDb);
            model.UserName = GetEmail();
            return View(model);
        }

        public ActionResult Add()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.USERS))
            {
                return RedirectToAction("Menu", "Home");
            }

            UserFormViewModel model = new UserFormViewModel();
            model.UserName = GetEmail();
            model.AllPermissions = GetAllPermissions();
            model.HasError = false;
            model.ActionType = ActionTypeEnum.ADD;
            return View("_Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(UserFormViewModel model, FormCollection formCollection)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.USERS))
            {
                return RedirectToAction("Menu", "Home");
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                ModelState.AddModelError("", "Należy podać imię");
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                ModelState.AddModelError("", "Należy podać nazwisko");
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("", "Należy podać adres e-mail");
            }
            else
            {
                if (!model.Email.Contains('@'))
                {
                    ModelState.AddModelError("", "Należy podać prawidłowy adres e-mail");
                }
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("", "Należy podać hasło");
            }
            if (string.IsNullOrEmpty(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Należy potwierdzić hasło");
            }
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Potwierdzenie nie jest zgodne z nowym hasłem");
            }
            if (_repository.GetByUserName(model.Email) != null)
            {
                ModelState.AddModelError("", "Konto z podanym adresem e-mail już istnieje");
            }

            if (ModelState.IsValid)
            {
                model.AllPermissions = GetAllPermissions();
                model.UserPermission = new List<string>();

                foreach (string permission in model.AllPermissions)
                {
                    string p = formCollection[permission];
                    if (p.StartsWith("true"))
                    {
                        model.UserPermission.Add(permission);
                    }
                };

                User userDb = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    IsActive = model.IsActive,
                    AddedAt = DateTime.Now,
                    Permissions = new List<Permission>()
                };

                foreach (string permission in model.UserPermission)
                {
                    Permission permissionDb = new Permission()
                    {
                        Permiss = (PermissionEnum)Enum.Parse(typeof(PermissionEnum), permission)
                    };
                    userDb.Permissions.Add(permissionDb);
                }

                bool result = _repository.Add(userDb, model.Password);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Nowe hasło musi zawierać przynajmniej jeden znak niebędący literą, cyfrę, jak również małą i wielką literę");
                }
            }

            model.UserName = GetEmail();
            model.AllPermissions = GetAllPermissions();
            model.HasError = true;
            model.ActionType = ActionTypeEnum.ADD;
            return View("_Form", model);
        }

        public ActionResult Edit(string id)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.USERS))
            {
                return RedirectToAction("Menu", "Home");
            }

            User userDb = _repository.GetById(id);
            UserFormViewModel model = UserFormViewModel.FromDb(userDb);
            model.UserName = GetEmail();
            model.AllPermissions = GetAllPermissions();
            model.ActionType = ActionTypeEnum.EDIT;
            return View("_Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, UserFormViewModel model, FormCollection formCollection)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.USERS))
            {
                return RedirectToAction("Menu", "Home");
            }

            User userDb = _repository.GetById(id);
            if (userDb == null)
            {
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                ModelState.AddModelError("", "Należy podać imię");
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                ModelState.AddModelError("", "Należy podać nazwisko");
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("", "Należy podać adres e-mail");
            }
            else
            {
                if (!model.Email.Contains('@'))
                {
                    ModelState.AddModelError("", "Należy podać prawidłowy adres e-mail");
                }
            }

            if (ModelState.IsValid)
            {
                model.AllPermissions = GetAllPermissions();
                model.UserPermission = new List<string>();

                foreach (string permission in model.AllPermissions)
                {
                    string p = formCollection[permission];
                    if (p.StartsWith("true"))
                    {
                        model.UserPermission.Add(permission);
                    }
                };

                _repository.RemovePermissionById(userDb);

                userDb.FirstName = model.FirstName;
                userDb.LastName = model.LastName;
                userDb.Email = model.Email;
                userDb.UserName = model.Email;
                userDb.IsActive = model.IsActive;

                foreach (string permission in model.UserPermission)
                {
                    Permission permissionDb = new Permission()
                    {
                        Permiss = (PermissionEnum)Enum.Parse(typeof(PermissionEnum), permission)
                    };
                    userDb.Permissions.Add(permissionDb);
                }

                _repository.Edit(userDb);
                return RedirectToAction("Index");
            }
            else
            {
                model.UserName = GetEmail();
                model.AllPermissions = GetAllPermissions();
                model.HasError = true;
                model.ActionType = ActionTypeEnum.EDIT;
                return View("_Form", model);
            }
        }

        public ActionResult PasswordReset(string id)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.USERS))
            {
                return RedirectToAction("Menu", "Home");
            }

            User userDb = _repository.GetById(id);
            if (userDb == null)
            {
                return RedirectToAction("Index");
            }

            string password = _repository.ResetPassword(userDb);
            Session[Configuration.Information] = $"Hasło dla użytkownika {userDb.Email} zostało zresetowane. Aktualne hasło to {password}.";
            return RedirectToAction("Index");
        }

        private List<string> GetAllPermissions()
        {
            List<string> permissions = new List<string>();

            foreach (PermissionEnum permission in Enum.GetValues(typeof(PermissionEnum)))
            {
                permissions.Add(permission.ToString());
            }

            return permissions;
        }
    }
}