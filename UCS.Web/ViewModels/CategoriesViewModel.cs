using System.Collections.Generic;
using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class CategoriesViewModel : LayoutViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }

        public static CategoriesViewModel FromDb(List<Category> categoriesDb)
        {
            CategoriesViewModel categoriesModel = new CategoriesViewModel();
            categoriesModel.Categories = new List<CategoryViewModel>();

            foreach (Category categoryDb in categoriesDb)
            {
                CategoryViewModel categoryModel = CategoryViewModel.FromDb(categoryDb);
                categoriesModel.Categories.Add(categoryModel);
            }

            return categoriesModel;
        }
    }
}