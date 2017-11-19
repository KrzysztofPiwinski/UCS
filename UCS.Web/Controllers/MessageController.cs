using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UCS.Db.Entities;
using UCS.Web.Models;
using UCS.Web.Models.DTOs;
using UCS.Web.Models.Repositories;
using UCS.Web.ViewModels;

namespace UCS.Web.Controllers
{
    public class MessageController : AbstractController
    {
        private CategoryRepository _categoryRepository = null;
        private MessageRepository _messageRepository = null;

        public MessageController() : base()
        {
            _categoryRepository = new CategoryRepository();
            _messageRepository = new MessageRepository();
        }

        public ActionResult Index(int page = 1)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.SEND_EMAIL))
            {
                return RedirectToAction("Menu", "Home");
            }

            if (page < 1)
            {
                page = 1;
            }

            List<Message> messagesDb;
            if (_permission.Contains(PermissionEnum.VIEW_ALL_EMAIL))
            {
                messagesDb = _messageRepository.GetAll();
            }
            else
            {
                messagesDb = _administrator.Messages.ToList();
            }

            int pageCount = (int)Math.Ceiling(messagesDb.Count / (double)Configuration.PageSize);

            if (pageCount > 0 && pageCount < page)
            {
                page = pageCount;
            }
            messagesDb = messagesDb.OrderByDescending(m => m.AddedAt).Skip((page - 1) * Configuration.PageSize).Take(Configuration.PageSize).ToList();

            MessagesViewModel model = MessagesViewModel.FromDb(messagesDb);
            model.CurrentPage = page;
            model.PageCount = pageCount;
            model.UserName = GetEmail();
            return View(model);
        }

        public ActionResult Add()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.SEND_EMAIL))
            {
                return RedirectToAction("Menu", "Home");
            }

            MessageFormViewModel model = new MessageFormViewModel();
            model.UserName = GetEmail();
            model.Categories = GetCategories();
            return View("_Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(MessageFormViewModel model)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_permission.Contains(PermissionEnum.SEND_EMAIL))
            {
                return RedirectToAction("Menu", "Home");
            }

            if (string.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("", "Należy podać tytuł wiadomości");
            }
            if (string.IsNullOrEmpty(model.Content))
            {
                ModelState.AddModelError("", "Należy podać treść wiadomości");
            }

            int categoryId;
            int.TryParse(model.Category, out categoryId);

            if (_categoryRepository.GetById(categoryId) == null)
            {
                ModelState.AddModelError("", "Nieprawidłowa kategoria wiadomości");
            }

            if (ModelState.IsValid)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    MessageDTO messageDTO = new MessageDTO()
                    {
                        Title = model.Title,
                        Content = model.Content,
                        CategoryId = categoryId,
                        UserId = _administrator.Id
                    };

                    httpClient.BaseAddress = new Uri("http://localhost:53645/");

                    StringContent content = new StringContent(JsonConvert.SerializeObject(messageDTO), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync("message/", content);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        response = await httpClient.PostAsync("message/", content);
                    }

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string count = await response.Content.ReadAsStringAsync();
                        Session[Configuration.Information] = $"Wiadomość została wysłana do {count} użytkowników";
                        return RedirectToAction("Menu", "Home");
                    }
                    else
                    {
                        Session[Configuration.Information] = "Wystąpił błąd. Proszę spróbować później.";
                        return RedirectToAction("Menu", "Home");
                    }
                }
            }

            model.HasError = true;
            model.UserName = GetEmail();
            return View("_Form", model);
        }

        private List<SelectListItem> GetCategories()
        {
            List<SelectListItem> categories = new List<SelectListItem>();

            List<Category> categoriesDb = _categoryRepository.GetAll();

            foreach (Category categoryDb in categoriesDb)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = categoryDb.Name,
                    Value = categoryDb.Id.ToString()
                };
                categories.Add(item);
            }

            return categories;
        }
    }
}