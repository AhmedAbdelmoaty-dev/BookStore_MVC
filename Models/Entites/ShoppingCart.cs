using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Models.identityEntites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entites
{
    public class ShoppingCart
    {
        [Key]
       public int ID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public int ProductID {  get; set; }
        [ValidateNever]
        public Product product { get; set; }
        [Range(1,1000,ErrorMessage ="please enter a value between 1 and 1000")]
        public int Count { get; set; }
        [ForeignKey(nameof(ApplicationUserID))]
        public string ApplicationUserID {  get; set; }
        [ValidateNever]
        public ApplicationUser applicationUser { get; set; }

        [NotMapped]
        public double Price {  get; set; }
    }
}
