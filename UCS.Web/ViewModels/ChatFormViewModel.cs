using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace UCS.Web.ViewModels
{
    public class ChatFormViewModel : LayoutViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string User { get; set; }
        public List<SelectListItem> Users { get; set; }
        public string Student { get; set; }
        public List<SelectListItem> Students { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}