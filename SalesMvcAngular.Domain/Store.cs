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
    
    public partial class Store
    {
        public Store()
        {
            this.Items = new HashSet<Item>();
            this.StoreTransactions = new HashSet<StoreTransaction>();
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
        }
    
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string Notes { get; set; }
    
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<StoreTransaction> StoreTransactions { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
