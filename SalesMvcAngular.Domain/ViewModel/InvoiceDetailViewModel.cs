using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMvcAngular.Domain.ViewModel
{
    public class InvoiceDetailViewModel
    {
        public string ItemName { get; set; }
        public decimal? ItemPrice { get; set; }
        public int? Quantity { get; set; }
        public string StoreName { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }

    }
}
