using BulkyWeb.Data;
using BulkyWeb.Models;
using BulkyWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
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
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
            if (obj.Name != null && obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "test is invalid Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Category objToUpdate = _unitOfWork.Category.Get(u => u.Id == id);
            //Category objToUpdate = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Category objToUpdate = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (objToUpdate == null)
            {
                return NotFound();
            }
            return View(objToUpdate);
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
            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category objToDelete = _unitOfWork.Category.Get(u => u.Id==id);
            if (objToDelete == null)
            {
                return NotFound();
            }
            return View(objToDelete);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            Category objToDelete = _unitOfWork.Category.Get(u => u.Id == id);
            if (objToDelete == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(objToDelete);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}











// This implementation is without using Repository Pattern


//using BulkyWeb.Data;
//using BulkyWeb.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace BulkyWeb.Controllers
//{
//    public class CategoryController : Controller
//    {
//        private readonly ApplicationDbContext _db;
//        public CategoryController(ApplicationDbContext db)
//        {
//            _db = db;
//        }
//        public IActionResult Index()
//        {
//            List<Category> objCategoryList = _db.Categories.ToList();
//            return View(objCategoryList);
//        }

//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult Create(Category obj)
//        {
//            if (obj.Name == obj.DisplayOrder.ToString())
//            {
//                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
//            }
//            if (obj.Name != null && obj.Name.ToLower() == "test")
//            {
//                ModelState.AddModelError("", "test is invalid Name");
//            }
//            if (ModelState.IsValid)
//            {
//                _db.Categories.Add(obj);
//                _db.SaveChanges();
//                TempData["success"] = "Category created successfully";
//                return RedirectToAction("Index");
//            }
//            return View();
//        }

//        public IActionResult Edit(int id)
//        {
//            if(id==0 || id==null)
//            {
//                return NotFound();
//            }
//            Category objToUpdate = _db.Categories.Find(id);
//            //Category objToUpdate = _db.Categories.FirstOrDefault(u => u.Id == id);
//            //Category objToUpdate = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
//            if (objToUpdate==null)
//            {
//                return NotFound();
//            }
//            return View(objToUpdate);
//        }
//        [HttpPost]
//        public IActionResult Edit(Category obj)
//        {
//            if(ModelState.IsValid)
//            {
//                _db.Categories.Update(obj);
//                _db.SaveChanges();
//                TempData["success"] = "Category updated successfully";
//                return RedirectToAction("Index");
//            }
//            return View();
//        }

//        public IActionResult Delete(int id)
//        {
//            if(id==null || id==0)
//            {
//                return NotFound();
//            }
//            Category objToDelete = _db.Categories.Find(id);
//            if(objToDelete==null)
//            {
//                return NotFound();
//            }
//            return View(objToDelete);
//        }
//        [HttpPost, ActionName("Delete")]
//        public IActionResult DeletePOST(int id)
//        {
//            Category objToDelete = _db.Categories.Find(id);
//            if (objToDelete == null)
//            {
//                return NotFound();
//            }
//            _db.Categories.Remove(objToDelete);
//            _db.SaveChanges();
//            TempData["success"] = "Category deleted successfully";
//            return RedirectToAction("Index");
//        }
//    }
//}
