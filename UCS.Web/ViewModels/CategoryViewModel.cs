using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static CategoryViewModel FromDb(Category categoryDb)
        {
            CategoryViewModel category = new CategoryViewModel()
            {
                Id = categoryDb.Id,
                Name = categoryDb.Name
            };
            return category;
        }
    }
}