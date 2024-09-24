using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : ControllerBase
    {
        private static List<CartItem> items = new List<CartItem>()
        {
            new CartItem() {Id=1, product="Apple", quantity=5, price=1.00},
            new CartItem() { Id = 2, product = "Banana", quantity = 3, price = 0.50 },
            new CartItem() { Id = 3, product = "Orange", quantity = 4, price = 0.75 },
            new CartItem() { Id = 4, product = "Grapes", quantity = 2, price = 2.00 },
            new CartItem() { Id = 5, product = "Watermelon", quantity = 2, price = 3.00 }
        };

        int nextId = 6;
        //Get cart-items

        // Curl commands:
        
        // curl -K 'GET' \
        // 'http://localhost:5225/api/CartItems?pageSize=2&page=1' \
        // -H 'accept: */*
        // curl -K 'GET' \ 
        //'http://localhost:5225/api/CartItems' \
        // -H 'accept: */*'
        // curl -k 'GET' \ 
        //'http://localhost:5225/api/CartItems?maxPrice=2' 
        // \ -H
        [HttpGet()]
        public IActionResult GetItems(double? maxPrice = null, string prefix = null, int? pageSize = null, int? page = null)
        {
            List<CartItem> result = items;

            // Includes products that are at or below maxPrice
            if (maxPrice.HasValue)
            {
                result = result.Where(item => item.price <= maxPrice.Value).ToList();
            }

            // Only includes products that start with the string
            if (!string.IsNullOrEmpty(prefix))
            {
                result = result.Where(item => item.product.StartsWith(prefix)).ToList();
            }
            // Extended, uses page query parameter
            if(pageSize.HasValue && page.HasValue)
            {
                int currentPage = page.Value;

                result = result.Skip(currentPage).Take(pageSize.Value).ToList();
            }

            // Only takes values up to the pageSize, which acts as a limit
            if (pageSize.HasValue)
            {
                result = result.Take(pageSize.Value).ToList();
            }

            return Ok(result);
        }
        // curl -X 'GET' \
        //  'http://localhost:5225/api/CartItems/2' \
        //   -H 'accept: */*'
        [HttpGet("{id}")]
        public IActionResult GetItemById(int id)
        {
            CartItem item = items.FirstOrDefault(c => c.Id == id);

            if (item == null)
            {
                return NotFound("ID Not Found");
            }
            else
            {
                return Ok(item);
            }
        }

    //  curl -K 'POST' \
    // 'http://localhost:5225/api/CartItems' \
    // -H 'accept: */*' \
    //  -H 'Content-Type: application/json' \
    // -d '{ "id": 5, "product": "Pizza", "price": 5, "quantity": 2}'
        [HttpPost()]
        public IActionResult AddItem([FromBody]CartItem newItem)
        {
            newItem.Id = nextId;
            items.Add(newItem);
            nextId++;
            return Created($"/api/CartItems/{newItem.Id}", newItem);
        }

        // curl -X 'PUT' \
        // 'http://localhost:5225/api/CartItems/6' \
        //  -H 'accept: */*' \
        // -H 'Content-Type: application/json' \
        // -d '{ "id": 3, "product": "Mango", "quantity": 1, "price": 1.50 }'

        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, [FromBody]CartItem updatedItem)
        {
            if (id != updatedItem.Id) {return BadRequest();}

            if(items.Any(c => c.Id == id) == false) {return NotFound();}

            int index = items.FindIndex(c => c.Id == id);
            items[index] = updatedItem;
            return Ok(updatedItem);
        }

        // curl -X 'DELETE' \
        // 'http://localhost:5225/api/CartItems/3' \
        //  -H 'accept: */*'
        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            int index = items.FindIndex(e => e.Id == id);

            if (index == -1)
            {
                return NotFound();
            }
            else
            {
                items.RemoveAt(index);
                return NoContent();
            }
        }
    }
}