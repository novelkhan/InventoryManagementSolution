using InventoryManagement.Common.Dtos;
using InventoryManagement.Core.Entities;
using InventoryManagement.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category?.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CreatedAt = p.CreatedAt,
                Discount = p.Discount
            }).ToList();
        }

        public async Task AddProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                CategoryId = (int)productDto.CategoryId,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                CreatedAt = DateTime.Now, // সার্ভারে সেট
                Discount = productDto.Discount
            };
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateProduct(int id, ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                product.Name = productDto.Name;
                product.CategoryId = (int)productDto.CategoryId; // আপডেট করা
                product.Price = productDto.Price;
                product.StockQuantity = productDto.StockQuantity;
                product.Discount = productDto.Discount;
                await _productRepository.UpdateAsync(product);
            }
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}