using System.Collections.Generic;
using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class StudentsViewModel : LayoutViewModel
    {
        public List<StudentViewModel> Students { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }

        public static StudentsViewModel FromDb(List<Student> studentsDb)
        {
            StudentsViewModel studentsModel = new StudentsViewModel()
            {
                Students = new List<StudentViewModel>()
            };

            foreach (Student studentDb in studentsDb)
            {
                StudentViewModel model = StudentViewModel.FromDb(studentDb);

                studentsModel.Students.Add(model);
            }

            return studentsModel;
        }
    }
}