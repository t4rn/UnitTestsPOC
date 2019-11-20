using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnitTestsPOC.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public Customer Customer { get; set; }
        public List<Asset> Assets { get; set; }
    }
}
