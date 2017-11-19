using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace UCS.Web.ViewModels
{
    public class MessageFormViewModel : LayoutViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}