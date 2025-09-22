using BulkyWeb.Data;
using BulkyWeb.Models;
using BulkyWeb.Models.ViewModels;
using BulkyWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            // Using ViewModel
            ProductVM productVM = new()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };

            //// Using ViewBag to pass the CategoryList to the View
            //ViewBag.CategoryList = CategoryList;

            ////Using ViewData
            //ViewData["CategoryList"] = CategoryList;

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }


        public IActionResult Edit(int id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Product objToUpdate = _unitOfWork.Product.Get(u => u.Id == id);
            //Product objToUpdate = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Product objToUpdate = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (objToUpdate == null)
            {
                return NotFound();
            }
            return View(objToUpdate);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
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
            Product objToDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (objToDelete == null)
            {
                return NotFound();
            }
            return View(objToDelete);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            Product objToDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (objToDelete == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(objToDelete);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
