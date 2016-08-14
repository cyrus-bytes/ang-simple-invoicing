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
    
    public partial class StoreTransaction
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int StoreId { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Notes { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Store Store { get; set; }
    }
}
