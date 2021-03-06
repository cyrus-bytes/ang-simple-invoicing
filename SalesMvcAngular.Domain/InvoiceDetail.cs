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
    
    public partial class InvoiceDetail
    {
        public int Id { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> HeadId { get; set; }
        public Nullable<decimal> itemprice { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> ItemNet { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<int> SupplierId { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Store Store { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual InvoiceHeader InvoiceHeader { get; set; }
    }
}
