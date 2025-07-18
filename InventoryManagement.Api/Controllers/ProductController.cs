﻿using InventoryManagement.Common.Dtos;
using InventoryManagement.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryManagement.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // ত্রুটির বিস্তারিত ফেরত
            }
            await _productService.AddProduct(product);
            var addedProduct = (await _productService.GetAllProducts()).Last(); // সর্বশেষ যোগ করা প্রোডাক্ট
            return CreatedAtAction(nameof(GetAll), new { id = addedProduct.Id }, addedProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto product)
        {
            await _productService.UpdateProduct(id, product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}