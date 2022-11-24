using Microsoft.AspNetCore.Mvc;
using NDBooks.DataAccess.Repository.IRepository;
using NDBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NDbookstore.Areas.Admin.Controllers
{
   
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)        // action method for upsert
        {
            Category category = new Category();

            if (id == null)
            {
                //this is for create
                return View(category);
            }

            //this is for edit
            category = _unitOfWork.Category.Get(id.GetValueOrDefault());

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // use HTTP POST to define the post-action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)   // check all variables in the model (e.g. Name Required) to increase security
            {
                if (category.id == 0)
                {
                    _unitOfWork.Category.Add(category);
                }
                else
                {
                    _unitOfWork.Category.update(category);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));     // to see all the categories
            }

            return View(category);
        }



        #region API CALLS
        [HttpGet]

        public IActionResult GetAll() 
        {
            var allObj = _unitOfWork.Category.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Category.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Category.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfully" });
        }
        #endregion
    }
}

