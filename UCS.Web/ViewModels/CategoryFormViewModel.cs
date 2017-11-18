using UCS.Db.Entities;
using UCS.Web.Models;

namespace UCS.Web.ViewModels
{
    public class CategoryFormViewModel : LayoutViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ActionTypeEnum ActionType { get; set; }

        public static CategoryFormViewModel FromDb(Category categoryDb)
        {
            CategoryFormViewModel category = new CategoryFormViewModel()
            {
                Id = categoryDb.Id,
                Name = categoryDb.Name
            };
            return category;
        }
    }
}