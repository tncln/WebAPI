using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPISample.DAL.Context;
using WebAPISample.DAL.Entities;

namespace WebAPISample.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            using var context = new WebAPIContext();
            return Ok(context.Categories.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using var context = new WebAPIContext();
            var category = context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPut]
        public IActionResult UpdateCategory(Category category)
        {
            using var context = new WebAPIContext();
            var updatedCategory = context.Categories.Find(category.Id);
            if (updatedCategory == null)
            {
                return NotFound();
            }
            updatedCategory.Name = category.Name;
            context.Update(updatedCategory);
            context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var context = new WebAPIContext();
            var deletedCategory = context.Categories.Find(id);
            if (deletedCategory == null)
            {
                return NotFound();
            }
            context.Remove(deletedCategory);
            context.SaveChanges();
            return NoContent();
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            using var context = new WebAPIContext();
            context.Categories.Add(category);
            context.SaveChanges();
            return Created("", category);
        }
        [HttpGet("{id}/blogs")]
        public IActionResult GetWithBlogsById(int id)
        {
            using var context = new WebAPIContext();
            var category = context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryWithBlogs= context.Categories.Where(x => x.Id == id).Include(I => I.Blogs).ToList();
            return Ok(categoryWithBlogs);
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var newFileName = Guid.NewGuid()+ Path.GetExtension(file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents/" + newFileName);
            var stream = new FileStream(path,FileMode.Create);
            await file.CopyToAsync(stream);
            return Created("",file);
        }
    }
}
