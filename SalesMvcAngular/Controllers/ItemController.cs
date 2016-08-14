using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesMvcAngular.Domain;
using SalesMvcAngular.Domain.Services;
using SalesMvcAngular.Domain.UOW;

namespace SalesMvcAngular.Controllers
{
    public class ItemController : Controller
    {
        private DropDownServices services = new DropDownServices();
        private UnitOfWork unitOfWork = new UnitOfWork();
        private List<Store> storeList;
        private List<Supplier> suppliersList;

        public ItemController()
        {
            storeList = unitOfWork.StoreRepository.GetAll().ToList();
            suppliersList = unitOfWork.SupplierRepository.GetAll().ToList();
        }
        //
        // GET: /Item/
        public ActionResult Index()
        {
            var allItems = unitOfWork.ItemRepository.GetAll().ToList();
            return View(allItems);
        }

        //
        // GET: /Item/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Item/Create
        public ActionResult Create()
        {
            ViewBag.SupplierID = services.GetSuppliers(suppliersList);
            ViewBag.StoreID = services.GetStores(storeList);
            return View();
        }

        //
        // POST: /Item/Create
        [HttpPost]
        public ActionResult Create(Item item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.ItemRepository.Add(item);

                   

                    if (unitOfWork.Save())
                    {
                        var trans = new StoreTransaction()
                        {
                            Date = DateTime.Now,
                            ItemId = item.Id,
                            Quantity = item.BeginningInventory,
                            StoreId = item.StoreID ?? 0,
                            Notes = "ادخال اصناف الى المخزن من صفحة اضافه منتج"
                        };
                        unitOfWork.StoreTransactionRepository.Add(trans);
                        if (unitOfWork.Save())
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }

                ViewBag.SupplierID = services.GetSuppliers(suppliersList);
                ViewBag.StoreID = services.GetStores(storeList);
                return View();

            }
            catch
            {
                ViewBag.SupplierID = services.GetSuppliers(suppliersList);
                ViewBag.StoreID = services.GetStores(storeList);
                return View();
            }
        }

        //
        // GET: /Item/Edit/5
        public ActionResult Edit(int id)
        {
            Item t = unitOfWork.ItemRepository.FindById(id);

            ViewBag.SupplierID = services.GetSuppliers(suppliersList, id.ToString());
            ViewBag.StoreID = services.GetStores(storeList, id.ToString());
            return View(t);
        }

        //
        // POST: /Item/Edit/5
        [HttpPost]
        public ActionResult Edit(Item item)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    unitOfWork.ItemRepository.Update(item);
                    if (unitOfWork.Save())
                    {
                        return RedirectToAction("Index");
                    }

                }
                return View();

            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Item/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //
        // POST: /Item/Delete/5
        [HttpPost]
        public JsonResult Delete(string itemId)
        {
            try
            {
                // TODO: Add delete logic here
                if (string.IsNullOrWhiteSpace(itemId))
                {
                    return new JsonResult() { Data = 0, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                var id = Convert.ToInt32(itemId);
                var item = unitOfWork.ItemRepository.FindById(id);
                unitOfWork.ItemRepository.Remove(item);
                if (unitOfWork.Save())
                {
                    return new JsonResult() { Data = 1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
            }
            catch
            {

            }
            return new JsonResult() { Data = 0, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult LoadItems()
        {
            //jQuery DataTables Param
            var draw = Request.Form.GetValues("draw").FirstOrDefault();

            var itemName = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var d = unitOfWork.InvoiceDetailsRepository.GetAll().ToList().Select(a => new
            {
                a.Item.ItemName,
                a.itemprice,
                a.Quantity,
                a.Store.StoreName,
                a.InvoiceHeader.Customer.CustomerName,
                a.InvoiceHeader.invoiceDate,
                TotalPrice = a.ItemNet
                //TotalPrice = ((a.Quantity * a.itemprice) - ((a.Discount * (a.Quantity * a.itemprice)) / 100)) + ((a.Tax * (a.Quantity * a.itemprice)/100)),
            }).ToList();
            return Json(new { draw = draw, data = d },
                     JsonRequestBehavior.AllowGet);

        }

        protected override void Dispose(bool disposing)
        {
            services.Dispose();
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
