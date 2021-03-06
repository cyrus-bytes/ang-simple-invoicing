//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SalesMvcAngular.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item
    {
        public Item()
        {
            this.StoreTransactions = new HashSet<StoreTransaction>();
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
        }
    
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal PurchasPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int BeginningInventory { get; set; }
        public string BarCode { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> StoreID { get; set; }
        public string Note { get; set; }
    
        public virtual Store Store { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<StoreTransaction> StoreTransactions { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
