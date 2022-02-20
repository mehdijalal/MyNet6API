using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.Models;
using MyDotNet6API.Classes;
using MyDotNet6API.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyDotNet6API.Controllers
{
    //https://localhost:7203/api/v1/ProductV1_?Page=3&Size=10
    //[Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ProductV1_0Controller : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductV1_0Controller(ShopContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }
        // GET: api/<ProductController>
        //[HttpGet]
        //public IEnumerable<Product> GetAllProducts()
        //{
        //    //return new string[] { "value1", "value2" };
        //    return _context.Products.ToArray();
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllProducts()
        //{
        //    //return new string[] { "value1", "value2" };
        //    var allProducts = await _context.Products.ToArrayAsync();
        //    return Ok(allProducts);
        //}

        //---adding paging----//
        //[HttpGet]
        //public async Task<IActionResult> GetAllProducts([FromQuery]QueryParameters queryParameters)
        //{

        //    IQueryable<Product> allProducts = _context.Products;

        //    allProducts = allProducts.Skip(queryParameters.Size * (queryParameters.Page - 1))
        //        .Take(queryParameters.Size);
        //    return Ok(await allProducts.ToArrayAsync());
        //}
        //------3------------//
        //[HttpGet]
        //public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        //{

        //    IQueryable<Product> products = _context.Products;

        //    if(queryParameters.MinPrice!=null && queryParameters.MaxPrice != null)
        //    {
        //        products=products.Where(p=>p.Price>=queryParameters.MinPrice.Value && p.Price<=queryParameters.MaxPrice.Value);
        //    }
        //    if (!string.IsNullOrEmpty(queryParameters.Sku))
        //    {
        //        products = products.Where(p=>p.Sku==queryParameters.Sku);
        //    }
        //    if (!string.IsNullOrEmpty(queryParameters.Name))
        //    {
        //        products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
        //    }
        //    products = products.Skip(queryParameters.Size * (queryParameters.Page - 1))
        //        .Take(queryParameters.Size);
        //    return Ok(await products.ToArrayAsync());
        //}


        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {

            IQueryable<Product> products = _context.Products;

            if (queryParameters.MinPrice != null && queryParameters.MaxPrice != null)
            {
                products = products.Where(p => p.Price >= queryParameters.MinPrice.Value && p.Price <= queryParameters.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                products = products.Where(p => p.Sku == queryParameters.Sku);
            }
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }
            //------We created helper method MyCustomOrderBy to make sorting easy, so below we use it-------//
            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
                {
                    products = products.MyCustomOrderBy(queryParameters.SortBy,queryParameters.SortOrder);
                }
            }
            products = products.Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);
            return Ok(await products.ToArrayAsync());
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody]Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                    "GetProduct", 
                    new { id = product.Id }, 
                    product);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product) 
        {
            if(id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State= EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (_context.Products.Find(id) == null)
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }
        // DELETE api/<ProductController>/ multiple product

        //example: https://localhost:7203/api/Product/Delete?ids=35&ids=36&ids=37
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteMultipleProduct([FromQuery] int[] ids)
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                var product = await _context.Products.FindAsync(id);
                if(product == null)
                {
                    return NotFound();
                }
                products.Add(product);
            }
            _context.Products.RemoveRange(products);
  
            await _context.SaveChangesAsync();
            return Ok(products);
        }
    }
    //---API v2----------------//
    //https://localhost:7203/api/v2/ProductV1_?Page=3&Size=10
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ProductV2_0Controller : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductV2_0Controller(ShopContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }
        // GET: api/<ProductController>
        //[HttpGet]
        //public IEnumerable<Product> GetAllProducts()
        //{
        //    //return new string[] { "value1", "value2" };
        //    return _context.Products.ToArray();
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllProducts()
        //{
        //    //return new string[] { "value1", "value2" };
        //    var allProducts = await _context.Products.ToArrayAsync();
        //    return Ok(allProducts);
        //}

        //---adding paging----//
        //[HttpGet]
        //public async Task<IActionResult> GetAllProducts([FromQuery]QueryParameters queryParameters)
        //{

        //    IQueryable<Product> allProducts = _context.Products;

        //    allProducts = allProducts.Skip(queryParameters.Size * (queryParameters.Page - 1))
        //        .Take(queryParameters.Size);
        //    return Ok(await allProducts.ToArrayAsync());
        //}
        //------3------------//
        //[HttpGet]
        //public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        //{

        //    IQueryable<Product> products = _context.Products;

        //    if(queryParameters.MinPrice!=null && queryParameters.MaxPrice != null)
        //    {
        //        products=products.Where(p=>p.Price>=queryParameters.MinPrice.Value && p.Price<=queryParameters.MaxPrice.Value);
        //    }
        //    if (!string.IsNullOrEmpty(queryParameters.Sku))
        //    {
        //        products = products.Where(p=>p.Sku==queryParameters.Sku);
        //    }
        //    if (!string.IsNullOrEmpty(queryParameters.Name))
        //    {
        //        products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
        //    }
        //    products = products.Skip(queryParameters.Size * (queryParameters.Page - 1))
        //        .Take(queryParameters.Size);
        //    return Ok(await products.ToArrayAsync());
        //}


        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {

            IQueryable<Product> products = _context.Products.Where(p=>p.IsAvailable==true);

            if (queryParameters.MinPrice != null && queryParameters.MaxPrice != null)
            {
                products = products.Where(p => p.Price >= queryParameters.MinPrice.Value && p.Price <= queryParameters.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                products = products.Where(p => p.Sku == queryParameters.Sku);
            }
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }
            //------We created helper method MyCustomOrderBy to make sorting easy, so below we use it-------//
            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
                {
                    products = products.MyCustomOrderBy(queryParameters.SortBy, queryParameters.SortOrder);
                }
            }
            products = products.Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);
            return Ok(await products.ToArrayAsync());
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                    "GetProduct",
                    new { id = product.Id },
                    product);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.Find(id) == null)
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }
        // DELETE api/<ProductController>/ multiple product

        //example: https://localhost:7203/api/Product/Delete?ids=35&ids=36&ids=37
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteMultipleProduct([FromQuery] int[] ids)
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                products.Add(product);
            }
            _context.Products.RemoveRange(products);

            await _context.SaveChangesAsync();
            return Ok(products);
        }
    }
}
