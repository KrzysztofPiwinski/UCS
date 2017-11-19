using System.Collections.Generic;
using System.Linq;
using UCS.Db;
using UCS.Db.Entities;

namespace UCS.Web.Models.Repositories
{
    public class MessageRepository
    {
        private UCSContext _context = null;

        public MessageRepository():base()
        {
            _context = new UCSContext();
        }

        public List<Message> GetAll()
        {
            return _context.Messages.ToList();
        }
    }
}