using System;
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
            return _context.Students.Single(s => s.Guid == guid);
        }

        public List<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public void Add(Student studentDb)
        {
            _context.Students.Add(studentDb);
            _context.SaveChanges();
        }

        public void Edit(Student studentDb)
        {
            studentDb.LastModifiedAt = DateTime.Now;
            _context.SaveChanges();
        }

        public void Delete(Student studentDb)
        {
            studentDb.IsActive = false;
            _context.SaveChanges();
        }
    }
}