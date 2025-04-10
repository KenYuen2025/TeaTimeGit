using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeaTimeDemoPractice.DataAccess.Data;
using TeaTimeDemoPractice.DataAccess.Repository.IRepository;
using TeaTimeDemoPractice.Models;
using TeaTimeDemoPractice.Utility;

namespace TeaTimeDemoPractice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Manager)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();

            //List<Category> objCategoryList = _categoryRepo.GetAll().ToList();

            //List<Category> objCategoryList = _db.Categories.ToList();

            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "類別名稱不能跟順序一致");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.save();

                //_categoryRepo.Add(obj);
                //_categoryRepo.Save();

                //_db.Categories.Add(obj);
                //_db.SaveChanges();

                TempData["success"] = "類別新增成功!";
                return RedirectToAction("Index");
            }
            return View();
        
        }

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            Category ? categoryFromDb = _unitOfWork.Category.Get(u => u.id == id);

            //Category? categoryFromDb = _categoryRepo.Get(u => u.id == id);

            //Category? categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View (categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.save();

                //_categoryRepo.Update(obj);
                //_categoryRepo.Save();

                //_db.Categories.Update(obj);
                //_db.SaveChanges();

                TempData["success"] = "更新成功!";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            Category categoryFromDb = _unitOfWork.Category.Get(u => u.id == id);

            //Category categoryFromDb = _categoryRepo.Get(u => u.id == id);

            //Category categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.id == id);

            //Category? obj = _categoryRepo.Get(u => u.id == id);

            //Category? obj = _db.Categories.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.save();

            //_categoryRepo.Remove(obj);
            //_categoryRepo.Save();

            //_db.Categories.Remove(obj);
            //_db.SaveChanges();

            TempData["success"] = "刪除成功!";
            return RedirectToAction("Index");
        }

    }
}
