using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REST_API.DataAccess.EntityFramework;
using REST_API.Models;

namespace REST_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
	private readonly ToDoContext _context;

	public ToDoItemsController(ToDoContext context)
	{
		_context = context;
	}

	// GET: api/ToDoItems
	[HttpGet]
	public async Task<ActionResult<IEnumerable<ToDoItemDTO>>> GetToDoItems()
	{
		if (_context.ToDoItems == null) return NotFound();

		return await _context.ToDoItems.Select(x => ItemToDTO(x)).ToListAsync();
	}

	// GET: api/ToDoItems/5
	[HttpGet("{id}")]
	public async Task<ActionResult<ToDoItemDTO>> GetToDoItem(long id)
	{
		if (_context.ToDoItems == null) return NotFound();

		ToDoItem? toDoItem = await _context.ToDoItems.FindAsync(id);

		if (toDoItem == null) return NotFound();

		return ItemToDTO(toDoItem);
	}

	// POST: api/ToDoItems
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPost]
	public async Task<ActionResult<ToDoItem>> CreateToDoItem(ToDoItemDTO toDoItemDTO)
	{
		if (_context.ToDoItems == null)
		{
			return Problem("Entity set 'ToDoContext.ToDoItems'  is null.");
		}

		ToDoItem toDoItem = new ToDoItem
		{
			Name = String.IsNullOrEmpty(toDoItemDTO.Name) ? String.Empty : toDoItemDTO.Name,
			IsComplete = toDoItemDTO.IsComplete

		};

		_context.ToDoItems.Add(toDoItem);
		await _context.SaveChangesAsync();

		// return CreatedAtAction("GetToDoItem", new { id = toDoItem.Id }, toDoItem);
		return CreatedAtAction(nameof(GetToDoItem), new
		{
			id = toDoItem.Id
		}, ItemToDTO(toDoItem));
	}

	// PUT: api/ToDoItems/5
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateToDoItem(long id, ToDoItemDTO toDoItemDTO)
	{
		if (id != toDoItemDTO.Id)
		{
			return BadRequest();
		}

		ToDoItem? toDoItem = await _context.ToDoItems.FindAsync(id);

		if (toDoItem == null)
		{
			return NotFound();
		}

		toDoItem.Name = String.IsNullOrEmpty(toDoItemDTO.Name) ? String.Empty : toDoItemDTO.Name;
		toDoItem.IsComplete = toDoItemDTO.IsComplete;

		_context.Entry(toDoItem).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException) when (!ToDoItemExists(id))
		{
			return NotFound();
		}

		return NoContent();
	}

	// DELETE: api/ToDoItems/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteToDoItem(long id)
	{
		if (_context.ToDoItems == null)
		{
			return NotFound();
		}

		var toDoItem = await _context.ToDoItems.FindAsync(id);

		if (toDoItem == null)
		{
			return NotFound();
		}

		_context.ToDoItems.Remove(toDoItem);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool ToDoItemExists(long id)
	{
		return (_context.ToDoItems?.Any(e => e.Id == id)).GetValueOrDefault();
	}

	private static ToDoItemDTO ItemToDTO(ToDoItem todoItem)
	{
		return new ToDoItemDTO
		{
			Id = todoItem.Id,
			Name = todoItem.Name,
			IsComplete = todoItem.IsComplete
		};
	}
}