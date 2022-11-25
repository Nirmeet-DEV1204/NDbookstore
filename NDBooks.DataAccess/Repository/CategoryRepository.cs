using NDBooks.DataAccess.Data;
using NDBooks.DataAccess.Repository.IRepository;
using NDBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDBooks.DataAccess.Repository
{
    public class CategoryRepository : Repostory<Category> , ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void update(Category category)
        {
            var objFromDb = _db.Categories.FirstOrDefault(S => S.id == category.id);
            if (objFromDb != null)
            {
                objFromDb.Name = category.Name;
                _db.SaveChanges();
            }
        }
    }
}
