using Microsoft.AspNetCore.Mvc;
using REST_API.DataAccess;
using REST_API.Models;

namespace REST_API.Controllers
{
	[Route("invoices/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICRUDBase<Category> _categoriesRepo;

		public CategoriesController(ICRUDBase<Category> categoriesRepo)
		{
			_categoriesRepo = categoriesRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IList<Category>>> GetAll()
		{
			IList<Category> categories = await _categoriesRepo.ReadAll();
			return Ok(categories);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Category>> GetCategoryById(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound();
			Category category = await _categoriesRepo.ReadById(new Guid(id));
			return Ok(category);
		}

		[HttpPost]
		public async Task<ActionResult<Category>> Create(Category model)
		{
			Category category = new Category(Guid.NewGuid(), model.Name);
			await _categoriesRepo.Create(category);
			return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(string id, Category model)
		{
			if (id != model.Id.ToString()) throw new ArgumentException(message: "Invalid parameter.", paramName: nameof(id));
			await _categoriesRepo.Update(model);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id, Category model)
		{
			if (id != model.Id.ToString()) throw new ArgumentException(message: "Invalid parameter.", paramName: nameof(id));
			Category categoryToDelete = await _categoriesRepo.ReadById(new Guid(id));
			if (categoryToDelete == null) return NotFound();
			await _categoriesRepo.Delete(new Guid(id));
			return NoContent();
		}
	}
}
