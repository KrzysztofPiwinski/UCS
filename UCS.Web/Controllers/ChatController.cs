using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using UCS.Db.Entities;
using UCS.Web.Models;
using UCS.Web.Models.Repositories;
using UCS.Web.ViewModels;

namespace UCS.Web.Controllers
{
    public class ChatController : AbstractController
    {
        private ChatRepository _chatRepository = null;
        private UserRepository _userRepository = null;
        private StudentRepository _studentRepository = null;

        public ChatController() : base()
        {
            _chatRepository = new ChatRepository();
            _userRepository = new UserRepository();
            _studentRepository = new StudentRepository();
        }

        public ActionResult Details(int id)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if ((_isAdministrator && !_permission.Contains(PermissionEnum.CHAT)) && !_isStudent)
            {
                return RedirectToAction("Menu", "Home");
            }

            if (_isAdministrator)
            {
                if (!_chatRepository.CheckIfIsMy(id, _administrator.Id))
                {
                    return RedirectToAction("Menu", "Home");
                }
            }
            else if (_isStudent)
            {
                if (!_chatRepository.CheckIfIsMy(id, studentId: _student.Id))
                {
                    return RedirectToAction("Menu", "Home");
                }
            }

            List<Chat> chatsDb = _chatRepository.GetAllThreadById(id);

            ChatsViewModel model = new ChatsViewModel();
            model.Title = chatsDb.First().Title;
            model.ParentChatId = chatsDb.First().ParentChatId;
            model.UserName = GetEmail();
            return View("Chat", model);
        }

        public ActionResult GetAwaitChatThread(int id)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if ((_isAdministrator && !_permission.Contains(PermissionEnum.CHAT)) && !_isStudent)
            {
                return RedirectToAction("Menu", "Home");
            }

            if (_isAdministrator)
            {
                if (!_chatRepository.CheckIfIsMy(id, _administrator.Id))
                {
                    return RedirectToAction("Menu", "Home");
                }
            }
            else if (_isStudent)
            {
                if (!_chatRepository.CheckIfIsMy(id, studentId: _student.Id))
                {
                    return RedirectToAction("Menu", "Home");
                }
            }

            List<Chat> chatsDb = null;

            if (_isAdministrator)
            {
                chatsDb = _chatRepository.GetAllThreadById(id, userId: _administrator.Id);
            }
            else if (_isStudent)
            {
                chatsDb = _chatRepository.GetAllThreadById(id, studentId: _student.Id);
            }

            ChatsViewModel model;
            if (_isStudent)
            {
                model = ChatsViewModel.FromDb(chatsDb, studentId: _student.Id);
            }
            else
            {
                model = ChatsViewModel.FromDb(chatsDb, userId: _administrator.Id);
            }
            model.UserName = GetEmail();
            return PartialView("_Chat", model);
        }

        public ActionResult Add(string content, int parentId)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if ((_isAdministrator && !_permission.Contains(PermissionEnum.CHAT)) && !_isStudent)
            {
                return RedirectToAction("Menu", "Home");
            }

            if (_isAdministrator)
            {
                if (!_chatRepository.CheckIfIsMy(parentId, _administrator.Id))
                {
                    return RedirectToAction("Menu", "Home");
                }
            }
            else if (_isStudent)
            {
                if (!_chatRepository.CheckIfIsMy(parentId, studentId: _student.Id))
                {
                    return RedirectToAction("Menu", "Home");
                }
            }

            Chat parentChatDb = _chatRepository.GetById(parentId);

            if (parentChatDb != null)
            {
                Chat chatDb = new Chat()
                {
                    ParentChatId = parentId,
                    Title = parentChatDb.Title,
                    Content = content,
                    SenderStudentId = _isStudent ? _student.Id : new int?(),
                    SenderUserId = _isAdministrator ? _administrator.Id : null
                };

                if (!string.IsNullOrEmpty(parentChatDb.SenderUserId) && (!_isAdministrator || _isAdministrator && parentChatDb.SenderUserId != _administrator.Id))
                {
                    chatDb.ReceiverUserId = parentChatDb.SenderUserId;
                }
                else if (parentChatDb.SenderStudentId != null && (!_isStudent || (_isStudent && parentChatDb.SenderStudentId != _student.Id)))
                {
                    chatDb.ReceiverStudentId = parentChatDb.SenderStudentId;
                }
                else if (!string.IsNullOrEmpty(parentChatDb.ReceiverUserId) && (!_isAdministrator || (_isAdministrator && parentChatDb.ReceiverUserId != _administrator.Id)))
                {
                    chatDb.ReceiverUserId = parentChatDb.ReceiverUserId;
                }
                else if (parentChatDb.ReceiverStudentId != null && (!_isStudent || (_isStudent && parentChatDb.ReceiverStudentId != _student.Id)))
                {
                    chatDb.ReceiverStudentId = parentChatDb.ReceiverStudentId;
                }

                _chatRepository.Add(chatDb);
            }

            //return RedirectToAction("GetAwaitChatThread", new { id = parentId });
            return new EmptyResult();
        }

        public ActionResult AdminStudentMessages(int page = 1)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if ((_isAdministrator && !_permission.Contains(PermissionEnum.CHAT)))
            {
                return RedirectToAction("Menu", "Home");
            }

            List<IGrouping<int, Chat>> threads = _chatRepository.GetAllStudentsForAdmin(_administrator.Id);

            //List<Chat> lastChat = new List<Chat>();
            //foreach (IGrouping<int, Chat> thread in threads)
            //{
            //    lastChat.Add(thread.SingleOrDefault(t => t.AddedAt == thread.Max(m => m.AddedAt)));
            //}

            //lastChat = lastChat.OrderByDescending(c => c.AddedAt).ToList();

            ChatThreadsViewModel model = ChatThreadsViewModel.FromDb(threads, _administrator.Id);
            model.UserName = GetEmail();
            model.ChatRecipient = ChatRecipientEnum.STUDENT;
            return View("Index", model);
        }

        public ActionResult StudentAdminMessages(int page = 1)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_isStudent)
            {
                return RedirectToAction("Menu", "Home");
            }

            List<IGrouping<int, Chat>> threads = _chatRepository.GetAllAdminsForStudent(_student.Id);

            //List<Chat> lastChat = new List<Chat>();
            //foreach (IGrouping<int, Chat> thread in threads)
            //{
            //    lastChat.Add(thread.SingleOrDefault(t => t.AddedAt == thread.Max(m => m.AddedAt)));
            //}

            //lastChat = lastChat.OrderByDescending(c => c.AddedAt).ToList();

            ChatThreadsViewModel model = ChatThreadsViewModel.FromDb(threads, studentId: _student.Id);
            model.UserName = GetEmail();
            model.ChatRecipient = ChatRecipientEnum.USER;
            return View("Index", model);
        }

        public ActionResult StudentStudentMessages(int page = 1)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_isStudent)
            {
                return RedirectToAction("Menu", "Home");
            }

            List<IGrouping<int, Chat>> threads = _chatRepository.GetAllStudentsForStudent(_student.Id);

            //List<Chat> lastChat = new List<Chat>();
            //foreach (IGrouping<int, Chat> thread in threads)
            //{
            //    lastChat.Add(thread.SingleOrDefault(t => t.AddedAt == thread.Max(m => m.AddedAt)));
            //}

            //lastChat = lastChat.OrderByDescending(c => c.AddedAt).ToList();

            ChatThreadsViewModel model = ChatThreadsViewModel.FromDb(threads, studentId: _student.Id);
            model.UserName = GetEmail();
            model.ChatRecipient = ChatRecipientEnum.STUDENT;
            return View("Index", model);
        }

        public ActionResult AdminAdminMessages()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if ((_isAdministrator && !_permission.Contains(PermissionEnum.CHAT)))
            {
                return RedirectToAction("Menu", "Home");
            }

            List<IGrouping<int, Chat>> threads = _chatRepository.GetAllAdminsForAdmin(_administrator.Id);

            //List<Chat> lastChat = new List<Chat>();
            //foreach (IGrouping<int, Chat> thread in threads)
            //{
            //    lastChat.Add(thread.SingleOrDefault(t => t.AddedAt == thread.Max(m => m.AddedAt)));
            //}

            //lastChat = lastChat.OrderByDescending(c => c.AddedAt).ToList();

            ChatThreadsViewModel model = ChatThreadsViewModel.FromDb(threads, _administrator.Id);
            model.UserName = GetEmail();
            model.ChatRecipient = ChatRecipientEnum.USER;
            return View("Index", model);
        }

        public ActionResult AddToStudent()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if ((_isAdministrator && !_permission.Contains(PermissionEnum.CHAT)) && !_isStudent)
            {
                return RedirectToAction("Menu", "Home");
            }

            ChatFormViewModel model = new ChatFormViewModel();
            model.Students = GetStudents();
            model.UserName = GetEmail();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToStudent(ChatFormViewModel model)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if ((_isAdministrator && !_permission.Contains(PermissionEnum.CHAT)) && !_isStudent)
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

            int studentId = int.Parse(model.Student);
            if (studentId == 0 || _studentRepository.GetById(studentId) == null)
            {
                ModelState.AddModelError("", "Nalezy wybrać adresata wiadomości");
            }

            if (ModelState.IsValid)
            {
                Chat chatDb = new Chat()
                {
                    Title = model.Title,
                    Content = model.Content,
                    ReceiverStudentId = studentId
                };

                if (_isAdministrator)
                {
                    chatDb.SenderUserId = _administrator.Id;
                }
                else if (_isStudent)
                {
                    chatDb.SenderStudentId = _student.Id;
                }

                _chatRepository.Add(chatDb);

                if (model.File != null)
                {
                    string guid = Guid.NewGuid().ToString("N");
                    ChatFile chatFileDb = new ChatFile()
                    {
                        RealFileName = model.File.FileName,
                        SavedFileName = guid + Path.GetExtension(model.File.FileName),
                        ChatId = chatDb.Id
                    };
                    chatDb.File = chatFileDb;

                    string path = Request.PhysicalApplicationPath + "Uploads\\" + guid + Path.GetExtension(model.File.FileName);
                    model.File.SaveAs(path);
                }

                chatDb.ParentChatId = chatDb.Id;
                _chatRepository.AddParentId(chatDb);

                return RedirectToAction("Details", new { id = chatDb.Id });
            }

            model.Students = GetStudents();
            model.HasError = true;
            return View(model);
        }

        public ActionResult AddToAdmin()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if ((_isAdministrator && !_permission.Contains(PermissionEnum.CHAT)) && !_isStudent)
            {
                return RedirectToAction("Menu", "Home");
            }

            ChatFormViewModel model = new ChatFormViewModel();
            model.Users = GetUsers();
            model.UserName = GetEmail();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToAdmin(ChatFormViewModel model)
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if ((_isAdministrator && !_permission.Contains(PermissionEnum.CHAT)) && !_isStudent)
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

            if (_userRepository.GetById(model.User) == null)
            {
                ModelState.AddModelError("", "Nalezy wybrać adresata wiadomości");
            }

            if (ModelState.IsValid)
            {
                Chat chatDb = new Chat()
                {
                    Title = model.Title,
                    Content = model.Content,
                    ReceiverUserId = model.User,
                };

                if (_isAdministrator)
                {
                    chatDb.SenderUserId = _administrator.Id;
                }
                else if (_isStudent)
                {
                    chatDb.SenderStudentId = _student.Id;
                }

                _chatRepository.Add(chatDb);

                if (model.File != null)
                {
                    string guid = Guid.NewGuid().ToString("N");
                    ChatFile chatFileDb = new ChatFile()
                    {
                        RealFileName = model.File.FileName,
                        SavedFileName = guid + Path.GetExtension(model.File.FileName),
                        ChatId = chatDb.Id
                    };
                    chatDb.File = chatFileDb;

                    string path = Request.PhysicalApplicationPath + "Uploads\\" + guid + Path.GetExtension(model.File.FileName);
                    model.File.SaveAs(path);
                }

                chatDb.ParentChatId = chatDb.Id;
                _chatRepository.AddParentId(chatDb);

                return RedirectToAction("Details", new { id = chatDb.Id });
            }

            model.Users = GetUsers();
            model.HasError = true;
            return View(model);
        }

        private List<SelectListItem> GetStudents()
        {
            List<SelectListItem> studentItems = new List<SelectListItem>();

            List<Student> studentsDb = _studentRepository.GetAll();
            if (!_isStudent)
            {
                foreach (Student studentDb in studentsDb)
                {
                    SelectListItem studentItem = new SelectListItem()
                    {
                        Value = studentDb.Id.ToString(),
                        Text = studentDb.FirstName + " " + studentDb.LastName
                    };

                    studentItems.Add(studentItem);
                }
            }
            else
            {
                foreach (Student studentDb in studentsDb)
                {
                    if (studentDb.Id != _student.Id)
                    {
                        SelectListItem studentItem = new SelectListItem()
                        {
                            Value = studentDb.Id.ToString(),
                            Text = studentDb.FirstName + " " + studentDb.LastName
                        };

                        studentItems.Add(studentItem);
                    }
                }
            }

            return studentItems;
        }

        private List<SelectListItem> GetUsers()
        {
            List<SelectListItem> userItems = new List<SelectListItem>();

            if (!_isAdministrator)
            {
                List<User> usersDb = _userRepository.GetAll();
                foreach (User userDb in usersDb)
                {
                    SelectListItem userItem = new SelectListItem()
                    {
                        Value = userDb.Id.ToString(),
                        Text = userDb.FirstName + " " + userDb.LastName + "(" + userDb.UserName + ")"
                    };

                    userItems.Add(userItem);
                }
            }
            else
            {
                List<User> usersDb = _userRepository.GetAll();
                foreach (User userDb in usersDb)
                {
                    if (userDb.Id != _administrator.Id)
                    {
                        SelectListItem userItem = new SelectListItem()
                        {
                            Value = userDb.Id.ToString(),
                            Text = userDb.FirstName + " " + userDb.LastName + "(" + userDb.UserName + ")"
                        };

                        userItems.Add(userItem);
                    }
                }
            }
            return userItems;
        }
    }
}