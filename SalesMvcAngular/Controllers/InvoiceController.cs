using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SalesMvcAngular.Domain;
using SalesMvcAngular.Domain.Services;
using SalesMvcAngular.Domain.UOW;
using SalesMvcAngular.Domain.ViewModel;

namespace SalesMvcAngular.Controllers
{
    public class InvoiceController : Controller
    {
        private DropDownServices services = new DropDownServices();
        private UnitOfWork unitOfWork = new UnitOfWork();
        private List<Store> storeList;
        private List<Supplier> suppliersList;

        public InvoiceController()
        {
            storeList = unitOfWork.StoreRepository.GetAll().ToList();
            suppliersList = unitOfWork.SupplierRepository.GetAll().ToList();
        }
        //
        // GET: /Invoice/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Invoice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public JsonResult GetCustomers()
        {
            var cl = unitOfWork.CustomersRepository.GetAll().ToList().Select(a => new
            {
                a.Id,
                a.CustomerName
            });
            return new JsonResult { Data = cl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetItems(string storeId)
        {

            if (!string.IsNullOrWhiteSpace(storeId))
            {
                int sId = Convert.ToInt32(storeId);
                var cl = unitOfWork.ItemRepository.Get(a => a.StoreID == sId).ToList().Select(a => new
          {
              a.Id,
              a.ItemName,
              a.SellPrice
          });
                return new JsonResult { Data = cl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return null;
        }
        public JsonResult GetStores()
        {
            var cl = unitOfWork.StoreRepository.GetAll().ToList().Select(a => new
            {
                a.Id,
                a.StoreName
            });
            return new JsonResult { Data = cl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult SaveInvoice(InvoiceHeader invHd, List<InvoiceDetail> invDt,string date)
        {
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(date))
            {
                invHd.invoiceDate = DateTime.ParseExact(date, "dd/MM/yyyy HH:MM", null);
            }
            unitOfWork.InvoiceHeaderRepository.Add(invHd);
            if (unitOfWork.Save())
            {
                foreach (var dt in invDt)
                {
                    dt.HeadId = invHd.Id;
                    unitOfWork.InvoiceDetailsRepository.Add(dt);
                    var itemTransaction =
                           unitOfWork.StoreTransactionRepository.Get(
                               a => a.ItemId == dt.ItemId && a.StoreId == dt.StoreId);

                    var itemQty = itemTransaction.Sum(a => a.Quantity);
                    var different = itemQty - dt.Quantity;
                    if (!itemTransaction.Any() || itemQty < 0 || different < 0)
                    {
                        var itemName = unitOfWork.ItemRepository.Get(a => a.Id == dt.ItemId).FirstOrDefault().ItemName;
                        // لا يوجد بالمخزن
                        result.AppendLine(string.Format(
                                    " هذا الصنف {0} غير موجود فى المخزن أو كميته فى المخزن غير كافية لاتمام العملية ",
                                    itemName));
                        result.AppendLine("<br/>");
                        continue;
                    }

                    var trans = new StoreTransaction()
                    {
                        Date = DateTime.Now,
                        ItemId = dt.ItemId ?? 0,
                        Quantity = dt.Quantity * -1,
                        StoreId = dt.StoreId ?? 0,
                        Notes = "اخراج اصناف من المخزن من صفحة المبيعات"
                    };

                    unitOfWork.Save();

                }

                return new JsonResult { Data = result.ToString(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


            }

            return new JsonResult { Data = 0, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult ItemMovement(string itemId)
        {
            Expression<Func<InvoiceDetail, bool>> predicate = d => true;
            if (!string.IsNullOrWhiteSpace(itemId))
            {
                if (itemId != "all")
                {
                    int id = Convert.ToInt32(itemId);
                    predicate = d => d.ItemId == id;
                }
               
            }

            // populate dropdown
            var lt = unitOfWork.StoreTransactionRepository.GetAll().ToList().Select(a => new SoldItems
            {
                ItemName = a.Item.ItemName,
                ItemId = a.ItemId
            }).Distinct().ToList();
            SelectListItem newItem = new SelectListItem { Text = "All", Value = "all" };
           
            ViewBag.listItem = services.GetSoldItems(lt);

            //populate items
            var items = unitOfWork.InvoiceDetailsRepository.Get(predicate).ToList().Select(a => new InvoiceDetailViewModel
            {
                ItemName = a.Item.ItemName,
                ItemPrice = a.itemprice,
                Quantity = a.Quantity ?? 0,
                StoreName = a.Store == null ? "" : a.Store.StoreName,
                CustomerName = a.InvoiceHeader.Customer.CustomerName,
                InvoiceDate = a.InvoiceHeader.invoiceDate == null ? "-" : a.InvoiceHeader.invoiceDate.ToString(),
                Discount = a.Discount == null ? "" : a.Discount.ToString() + " %",
                Tax = a.Tax == null ? "" : a.Tax.ToString() + " %",
                TotalPrice = a.ItemNet
            }).ToList();

            ViewBag.Items = items;           
            return View();
        }
    
        //
        // GET: /Invoice/Create
        public ActionResult Add()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddInvoice()
        {
            ViewBag.SupplierId = services.GetSuppliers(suppliersList);
            ViewBag.StoreID = services.GetStores(storeList);
            return View();
        }
        [HttpPost]
        public ActionResult AddInvoice(InvoiceHeader invoiceHeader)
        {

            return View();
        }
        //
        // POST: /Invoice/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Invoice/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Invoice/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Invoice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Invoice/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        protected override void Dispose(bool disposing)
        {
            services.Dispose();
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
