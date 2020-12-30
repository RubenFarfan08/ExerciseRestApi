using System.Collections;
using System.Threading.Tasks;
using Exercise.Data.Model;
using Exercise.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Exercise.Services
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Products>> GetProducts();        
        Task<Products> GetProductsByID(int productId);        
        Task InsertProduct(Products product);        
        Task DeleteProduct(int productId);        
        void UpdateProduct(Products product);      
        Task<bool> ProductExists(int id);  
        Task Save(); 
    }

    public class ProductsRepository : IProductsRepository
    {
         private readonly DataContext context;        
 
        public ProductsRepository(DataContext context)        
        {            
            this.context = context;        
        }        
        
        public async Task<IEnumerable<Products>> GetProducts()        
        {
            return await context.Products.ToListAsync();
                    
        }        
        public async Task<Products> GetProductsByID(int productId)        
        {
            return await context.Products.FindAsync(productId);
        }
        
        public async Task InsertProduct(Products products)
        {            
            await context.Products.AddAsync(products);  
        }        
        
        public async Task DeleteProduct(int productId)        
        {            
            Products Products = await context.Products.FindAsync(productId);                    
            context.Products.Remove(Products);        
        }        
        
        public void UpdateProduct(Products products)        
        {            
            context.Entry(products).State = EntityState.Modified;        
        }        
        
        public async Task Save()        
        {            
           await context.SaveChangesAsync();        
        }

        public async Task<bool> ProductExists(int id)
        {
            return await context.Products.AnyAsync(e => e.Id == id);
        }
    }
}