using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesMvcAngular.Domain.Services
{
    public class DropDownServices : IDisposable
    {
        public IEnumerable<SelectListItem> GetSuppliers(List<Supplier> suppliers,string selectedValue = null)
        {
            return suppliers.Select(a => new SelectListItem()
            {
                Text = a.SupplierName,
                Value = a.Id.ToString(),
                Selected = selectedValue == a.Id.ToString() ? true : false
            });
        }

        public IEnumerable<SelectListItem> GetStores(List<Store> stores, string selectedValue = null)
        {
            return stores.Select(a => new SelectListItem()
            {
                Text = a.StoreName,
                Value = a.Id.ToString(),
                Selected = selectedValue == a.Id.ToString() ? true : false
            });
        }
        public IEnumerable<SelectListItem> GetCustomers(List<Customer> customers, string selectedValue = null)
        {
            return customers.Select(a => new SelectListItem()
            {
                Text = a.CustomerName,
                Value = a.Id.ToString(),
                Selected = selectedValue == a.Id.ToString() ? true : false
            });
        }
        public IEnumerable<SelectListItem> GetSoldItems(List<SoldItems> soldItems, string selectedValue = null)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list = soldItems.Select(a => new SelectListItem()
            {
                Text = a.ItemName,
                Value = a.ItemId.ToString(),
                Selected = selectedValue == a.ItemId.ToString() ? true : false
            }).ToList();
            list.Add(new SelectListItem(){Text = "All",Value = "all"});
            return list.AsEnumerable();
        }
        public void Dispose()
        {
           GC.SuppressFinalize(this);
        }
    }

    public class SoldItems
    {
        public string ItemName { get; set; }
        public int ItemId { get; set; }
    }
}