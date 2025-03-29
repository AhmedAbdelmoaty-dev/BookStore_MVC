using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entites
{
    public class OrderDetail
    {
        public int ID { get; set; }
        public int OrderHeaderID {  get; set; }
        [ForeignKey("OrderHeaderID")]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        public int ProductID {  get; set; }
        [ForeignKey("ProductID")]
        [ValidateNever]
        public Product Product { get; set; }
        public int Count { get; set; }
        public double Price {  get; set; }

    }
}
