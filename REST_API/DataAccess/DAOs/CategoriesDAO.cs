using Microsoft.EntityFrameworkCore;
using REST_API.DataAccess.EntityFramework;
using REST_API.Extras;
using REST_API.Models;

namespace REST_API.DataAccess;

public class CategoriesDAO : ICRUDBase<Category>
{
	private readonly InvoicesDBContext _context;

	public CategoriesDAO(InvoicesDBContext context)
	{
		_context = context;
	}

	public async Task<IList<Category>> ReadAll()
	{
		if (_context.Invoices == null) throw new NullReferenceException("Entity set 'InvoicesDBContext.Categories'  is null.");

		return await _context.Categories.Select(c => ToDomainModel(c)).ToListAsync();
	}

	public async Task<Category> ReadById(Guid id)
	{
		if (_context.Categories == null) throw new NullReferenceException("Entity set 'InvoicesDBContext.Categories'  is null.");

		Categories? categoryDB = await _context.Categories.FindAsync(id);

		if (categoryDB == null) throw new NullReferenceException("Category not found.");

		return ToDomainModel(categoryDB);
	}

	public async Task Create(Category category)
	{
		if (_context.Invoices == null) throw new NullReferenceException("Entity set 'InvoicesDBContext.Categories'  is null.");

		Categories? categoryDB = ToDBModel(category);

		_context.Categories.Add(categoryDB);
		_context.Entry(categoryDB).State = EntityState.Added;

		await _context.SaveChangesAsync();
	}

	public async Task Update(Category category)
	{
		try
		{
			Categories? categoryDB = await _context.Categories.FindAsync(category.Id);

			if (categoryDB == null) throw new NullReferenceException("Category not found.");

			categoryDB.Nombre = String.IsNullOrEmpty(category.Name) ? String.Empty : category.Name;

			_context.Categories.Update(categoryDB);
			_context.Entry(categoryDB).State = EntityState.Modified;

			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException) when (!CategoryExists(category.Id.ToString()))
		{
			throw new NullReferenceException("Category already exists");
		}
	}

	public async Task Delete(Guid id)
	{
		if (_context.Categories == null) throw new NullReferenceException("Entity set 'InvoicesDBContext.Categories'  is null.");

		Categories? categoryDB = await _context.Categories.FindAsync(id);

		if (categoryDB == null) throw new NullReferenceException("Category not found.");

		_context.Categories.Remove(categoryDB);
		_context.Entry(categoryDB).State = EntityState.Deleted;

		await _context.SaveChangesAsync();
	}

	public static Category ToDomainModel(Categories categoryDB)
	{
		Category category = new Category(categoryDB.Id, categoryDB.Nombre);
		return category;
	}

	public static Categories ToDBModel(Category category)
	{
		Categories categoryDB = new Categories
		{
			Id = category.Id,
			Nombre = String.IsNullOrEmpty(category.Name) ? String.Empty : category.Name
		};

		return categoryDB;
	}

	private bool CategoryExists(string id)
	{
		return (_context.Categories?.Any(e => e.Id.Equals(new Guid(id)))).GetValueOrDefault();
	}
}
