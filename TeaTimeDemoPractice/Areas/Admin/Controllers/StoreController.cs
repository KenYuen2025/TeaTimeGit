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
    public class StoreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;
   
        public StoreController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Store> objStoreList = _unitOfWork.Store.GetAll().ToList();

            return View(objStoreList);
        }

        public IActionResult Upsert(int? id)
        {
           
            if (id == null || id == 0)
            {
                return View(new Store());
            }
            else
            {
                Store storeObj = _unitOfWork.Store.Get(u => u.Id == id);
                return View(storeObj);

            }
               
        }

        [HttpPost]
        public IActionResult Upsert(Store storeObj)
        {
            if (ModelState.IsValid)
            {
                if(storeObj.Id == 0)
                {
                    _unitOfWork.Store.Add(storeObj);
                }
                else
                {
                    _unitOfWork.Store.Update(storeObj);
                }

                _unitOfWork.save();
                TempData["success"] = "店舖新增成功!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(storeObj);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Store> objStoreList = _unitOfWork.Store.GetAll().ToList();
            return Json(new { data = objStoreList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var storeToBeDeleted = _unitOfWork.Store.Get(u => u.Id == id);

            if (storeToBeDeleted == null)
            {
                return Json(new { success = false, message = "刪除失敗" });
            }
            
            _unitOfWork.Store.Remove(storeToBeDeleted);
            _unitOfWork.save();
            return Json(new { success = true, message = "刪除成功" });
        }
        #endregion

    }


}
