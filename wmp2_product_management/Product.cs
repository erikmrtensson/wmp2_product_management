using System;
using System.Collections.Generic;
using System.Text;

namespace wmp2_product_management
{
    internal class Product
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
