using System;
using System.Collections.Generic;
using System.Text;

namespace BestBy
{
    internal class ProductGroup : List<Product>
    {
        public string Name { get; set; }

        public ProductGroup(string name, List<Product> products) : base(products)
        {
            Name = name;
        }
    }
}
