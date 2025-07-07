using InventoryManagement.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Infrastructure.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProducts();
        Task AddProduct(ProductDto productDto);
        Task UpdateProduct(int id, ProductDto productDto);
        Task DeleteProduct(int id);
    }
}
