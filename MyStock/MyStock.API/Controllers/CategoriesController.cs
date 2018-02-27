using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MyStock.API.Models;
using MyStock.Domain;

namespace MyStock.API.Controllers
{
    [Authorize]
    public class CategoriesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Categories
        public async Task<IHttpActionResult> GetCategories()
        {
            var categories = await db.Categories.ToListAsync();
            var categoriesResponse = new List<CategoryResponse>();

            foreach (var item in categories)
            {
                var productsResponse = new List<ProductResponse>();
                foreach (var itemProd in item.Productos)
                {
                    productsResponse.Add(new ProductResponse
                    {
                        ProductId = itemProd.ProductId,
                        Description = itemProd.Description,
                        Image = itemProd.Image,
                        IsActive = itemProd.IsActive,
                        LastPurchase = itemProd.LastPurchase,
                        Price = itemProd.Price,
                        Stock = itemProd.Stock,
                        Remarks = itemProd.Remarks,
                    });
                }
                categoriesResponse.Add(new CategoryResponse
                {
                    CategoryId = item.CategoryId,
                    Description = item.Description,
                    Productos = productsResponse,
                });
            }

            return Ok(categoriesResponse);
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if(e.InnerException != null && e.InnerException.InnerException != null && e.InnerException.InnerException.Message.Contains("Index"))
                {
                    return BadRequest("There are a record with the same description.");
                }
                return BadRequest(e.Message);
            }

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> DeleteCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }
    }
}