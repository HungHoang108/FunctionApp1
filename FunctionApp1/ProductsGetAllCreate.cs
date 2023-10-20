using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace FunctionApp1
{
    public class ProductsGetAllCreate
    {
        private readonly AppDbContext _db;

        public ProductsGetAllCreate(AppDbContext db)
        {
            _db = db;
        }

        [FunctionName("ProductsGetAllCreate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "products")] HttpRequest req)
        {
            if(req.Method == HttpMethods.Post)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonConvert.DeserializeObject<Product>(requestBody);
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return new CreatedResult("/products", product);
            } 

            var products = await _db.Products.ToListAsync();
            return new OkObjectResult(products);
        }
    }
}
