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
        protected bool IsStudent;
        protected Administrator _administrator;
        protected bool IsAdministrator;

        public AbstractController()
        {
            _context = new UCSContext();
        }

        public bool Init(IPrincipal user)
        {
            if (user != null && user.Identity.IsAuthenticated)
            {
                IsAdministrator = true;
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
                        return true;
                    }
                }
                return false;
            }
        }
    }
}