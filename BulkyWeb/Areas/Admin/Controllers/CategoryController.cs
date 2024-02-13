using Bulky.DataAccess.Repository.IRepository;
using BulkyWeb.DataAccess.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objectCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objectCategoryList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name != null && obj.Name.Equals("Kira", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Name", "Kira is not allowed");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public IActionResult Delete(int id)
        {
            Category? categoryDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryDb == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(categoryDb);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
