using System.Collections.Generic;
using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class MenuViewModel : LayoutViewModel
    {
        public List<PermissionEnum> Permission { get; set; }
    }
}