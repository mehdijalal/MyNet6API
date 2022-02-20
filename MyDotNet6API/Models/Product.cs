using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        /*---I add a property with the category ID as an int as a reference and then we can also have a virtual category property of type category. 
      * However there is a problem if you do it like this. We're using .NET Core 3.1 and .NET 3 got a new JSON support. 
      * Now imagine we have a lot of data in here. For instance we have a category and the category contains a couple of products. 
      * For example product A, product A now has here a reference to its category. The category however once again has a list of products including product A which in turn gives us a reference back to category. 
      * So we might have a kind of infinite loop here. 
      * It would have worked with the old JSON support and there are ways to make this work 
      * but for the purpose of our API what I am doing here is just I'm adding JSONIgnore as an attribute I do Control dot and then get the information that we should use--*/
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }
     

    }
}
