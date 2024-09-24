using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAPI.Models
{
    public class CartItem
    {
        public int Id {get; set;}
        public string product {get; set;}
        public double price {get; set;}
        public int quantity {get; set;}
    }
}