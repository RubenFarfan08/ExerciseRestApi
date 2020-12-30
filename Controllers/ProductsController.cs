using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exercise.Data;
using Exercise.Data.Model;
using Exercise.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
namespace Exercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController: ControllerBase
    {
        private IProductsRepository _products;
        public ProductsController(IProductsRepository products){
            _products = products;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts() {
            var products = await _products.GetProducts();
            if(products == null){
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProducts(int id) {
            if (ModelState.IsValid){
                var products = await _products.GetProductsByID(id);
                if(products == null){
                    return NotFound();
                }
                return Ok(products);
            }
            return BadRequest("Some properties are not valid");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(int id, Products products){
            if (ModelState.IsValid){
                if(id != products.Id){
                    return BadRequest();
                }
                _products.UpdateProduct(products);
                try{
                    await _products.Save();
                }catch (DbUpdateConcurrencyException)
                {
                    if (!await _products.ProductExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(products);
            }
            return BadRequest("Some properties are not valid");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(Products products){
            if (ModelState.IsValid){
                await _products.InsertProduct(products);
                await _products.Save();
                return Ok(products);
            }

            return BadRequest("Some properties are not valid");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Products>> DeleteProducts(int id){
            if (ModelState.IsValid){
                var product = await _products.GetProductsByID(id);
                if(product == null) {
                    return NotFound();
                }
                await _products.DeleteProduct(id);
                await _products.Save();
                return Ok(product);
            }
            return BadRequest("Some properties are not valid");
        }

        
    }
}