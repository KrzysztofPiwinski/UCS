using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCS.Db;
using UCS.Db.Entities;

namespace UCS.Web.Models.Repositories
{
    public class CategoryRepository
    {
        private UCSContext _context = null;

        public CategoryRepository() : base()
        {
            _context = new UCSContext();
        }

        public Category GetById(int id)
        {
            return _context.Categories.SingleOrDefault(c => c.Id == id);
        }

        public Category GetByName(string name)
        {
            return _context.Categories.SingleOrDefault(c => c.Name == name);
        }

        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public void Add(Category categoryDb)
        {
            _context.Categories.Add(categoryDb);
            _context.SaveChanges();
        }

        internal void Edit(Category categoryDb)
        {
            _context.SaveChanges();
        }
    }
}