using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using TeaTimeDemoPractice.DataAccess.Data;
using TeaTimeDemoPractice.DataAccess.Repository.IRepository;
using TeaTimeDemoPractice.Models;
using TeaTimeDemoPractice.Models.ViewModels;
using TeaTimeDemoPractice.Utility;

namespace TeaTimeDemoPractice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Manager)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;
   
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        /*private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }&/

        /*private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }*/

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

            //List<Category> objCategoryList = _categoryRepo.GetAll().ToList();

            //List<Category> objCategoryList = _db.Categories.ToList();

            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.name,
                    Value = u.id.ToString()
                }),
                Product = new Product()

            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);

            }
               
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if(productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                  
                _unitOfWork.save();
                TempData["success"] = "產品新增成功!";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.name,
                    Value = u.id.ToString()
                });

                return View(productVM);
            }
        }

        //public IActionResult Create()
        //{
        //    ProductVM productVM = new()
        //    {
        //        CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.name,
        //            Value = u.id.ToString()
        //        }),
        //        Product = new Product()

        //    };

            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString()
            //});

            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;

            //return View(productVM);
        //}

        //[HttpPost]
        //public IActionResult Create(ProductVM productVM)
        //{
        //    //if (obj.name == obj.DisplayOrder.ToString())
        //    //{
        //    //    ModelState.AddModelError("name", "類別名稱不能跟順序一致");
        //    //}

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(productVM.Product);
        //        _unitOfWork.save();

        //        //_categoryRepo.Add(obj);
        //        //_categoryRepo.Save();

        //        //_db.Categories.Add(obj);
        //        //_db.SaveChanges();

        //        TempData["success"] = "產品新增成功!";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.name,
        //            Value = u.id.ToString()
        //        });

        //        return View(productVM);

        //    }
            
        //}

        /*public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            Product ? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            //Category? categoryFromDb = _categoryRepo.Get(u => u.id == id);

            //Category? categoryFromDb = _db.Categories.Find(id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View (productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.save();

                //_categoryRepo.Update(obj);
                //_categoryRepo.Save();

                //_db.Categories.Update(obj);
                //_db.SaveChanges();

                TempData["success"] = "產品更新成功!";
                return RedirectToAction("Index");
            }

            return View();
        }*/

        

        //public IActionResult Delete(int? id)
        //{
        //    if(id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

        //    //Category categoryFromDb = _categoryRepo.Get(u => u.id == id);

        //    //Category categoryFromDb = _db.Categories.Find(id);

        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productFromDb);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u => u.Id == id);

        //    //Category? obj = _categoryRepo.Get(u => u.id == id);

        //    //Category? obj = _db.Categories.Find(id);

        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }

        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.save();

        //    //_categoryRepo.Remove(obj);
        //    //_categoryRepo.Save();

        //    //_db.Categories.Remove(obj);
        //    //_db.SaveChanges();

        //    TempData["success"] = "產品刪除成功!";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);

            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "刪除失敗" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.save();
            return Json(new { success = true, message = "刪除成功" });

        }

        #endregion

    }


}
