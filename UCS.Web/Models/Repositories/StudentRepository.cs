using System.Collections.Generic;
using System.Linq;
using UCS.Db;
using UCS.Db.Entities;

namespace UCS.Web.Models.Repositories
{
    public class StudentRepository
    {
        private UCSContext _context = new UCSContext();

        public Student GetByGuid(string guid)
        {
            return _context.Students.SingleOrDefault(s => s.Guid == guid);
        }

        public Student GetById(int id)
        {
            return _context.Students.SingleOrDefault(s => s.Id == id);
        }

        public List<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public List<Student> GetPage(int page)
        {
            return _context.Students
                .OrderBy(s => s.DeletedAt.HasValue)
                .ThenBy(s => s.UserName)
                .Skip((page - 1) * Configuration.PAGE_SIZE)
                .Take(Configuration.PAGE_SIZE)
                .ToList();
        }

        public void Add(Student studentDb)
        {
            _context.Students.Add(studentDb);
            _context.SaveChanges();
        }

        public void AddCategory(Student studentDb)
        {
            _context.StudentCategories.AddRange(studentDb.Categories);
            _context.SaveChanges();
        }

        public void Edit(Student studentDb)
        {
            _context.SaveChanges();
        }

        public void RemoveCategory(Student studentDb)
        {
            _context.StudentCategories.RemoveRange(studentDb.Categories);
            _context.SaveChanges();
        }
    }
}