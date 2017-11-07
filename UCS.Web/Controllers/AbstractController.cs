using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using UCS.Db;
using UCS.Db.Entities;

namespace UCS.Web.Controllers
{
    public abstract class AbstractController : Controller
    {
        protected UCSContext _context;
        protected Student _student;
        protected bool _isStudent;
        protected Administrator _administrator;
        protected bool _isAdministrator;
        protected List<PermissionEnum> _permission { get; set; }

        public AbstractController()
        {
            _context = new UCSContext();
        }

        public bool Init(IPrincipal user)
        {
            if (user != null && user.Identity.IsAuthenticated)
            {
                string userName = user.Identity.Name;
                _administrator = _context.Users.SingleOrDefault(u => u.UserName == userName);
                _isAdministrator = true;
                _permission = _administrator.Permissions.Select(a => a.Permiss).ToList();
                return true;
            }
            else
            {
                var guid = Session["ucs_student_guid"];

                if (guid != null)
                {
                    Student studentDb = _context.Students.SingleOrDefault(s => s.Guid == guid.ToString());
                    if (studentDb != null)
                    {
                        _student = studentDb;
                        _isStudent = true;
                        _permission = new List<PermissionEnum>()
                        {

                        };
                        return true;
                    }
                }
                return false;
            }
        }

        public string GetEmail()
        {
            if (_student != null)
            {
                return _student.UserName;
            }
            if (_administrator != null)
            {
                return _administrator.UserName;
            }
            return null;
        }
    }
}