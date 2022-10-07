using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using REST_API.DataAccess;
using REST_API.Extras;
using REST_API.Models;

namespace REST_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class InvoicesController : ControllerBase
	{
		private readonly ICRUDBase<Invoice> _invoicesRepo;
		private readonly ICRUDBase<Category> _categoriesRepo;

		public InvoicesController(ICRUDBase<Invoice> invoicesRepo, ICRUDBase<Category> categoriesRepo)
		{
			_invoicesRepo = invoicesRepo;
			_categoriesRepo = categoriesRepo;
		}

		// GET: /invoices
		[HttpGet]
		public async Task<ActionResult<IList<InvoiceResponseModel>>> GetAll()
		{
			try
			{
				IList<Invoice> invoices = await _invoicesRepo.ReadAll();
				IEnumerable<InvoiceResponseModel> response = invoices.Select(i => new InvoiceResponseModel(i));
				return Ok(response);
			}
			catch (NullReferenceException)
			{
				return NotFound();
			}
		}

		// GET: /invoices/jhds-ytrf-...
		[HttpGet("{id}")]
		public async Task<ActionResult<Invoice>> GetInvoice(string id)
		{
			try
			{
				if (string.IsNullOrEmpty(id)) return NotFound();

				Invoice invoice = await _invoicesRepo.ReadById(new Guid(id));

				InvoiceResponseModel response = new InvoiceResponseModel(invoice);

				return Ok(response);
			}
			catch (NullReferenceException)
			{
				return NotFound();
			}
		}

		// POST: /invoices
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Invoice>> Create(CreateInvoiceRequestModel model)
		{
			try
			{
				Invoice invoice = await model.ToDomainModel(_categoriesRepo);

				await _invoicesRepo.Create(invoice);

				// return CreatedAtAction("GetToDoItem", new { id = toDoItem.Id }, toDoItem);
				return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
			}
			catch (NullReferenceException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (FormatException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT: /invoices/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(string id, UpdateInvoiceRequestModel model)
		{
			try
			{
				if (id != model.Id) throw new NullReferenceException(message: nameof(id));

				Invoice invoice = await model.ToDomainModel(_categoriesRepo);

				await _invoicesRepo.Update(invoice);

				return NoContent();
			}
			catch (NullReferenceException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (FormatException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// DELETE: /invoices/5
		// [HttpDelete("{id}")]
		// public async Task<IActionResult> Delete(string id)
		// {
		// 	try
		// 	{
		// 		if (string.IsNullOrEmpty(id)) return NotFound();

		// 		await _InvoicesRepo.Delete(new Guid(id));

		// 		return NoContent();
		// 	}
		// 	catch (NullReferenceException)
		// 	{
		// 		return NotFound();
		// 	}
		// }

		// GET: /invoices/paymentmethods
		[HttpGet]
		[Route("paymentmethods")]
		public IActionResult GetPaymentMethods()
		{
			try
			{
				IList<string> paymentMethods = Enum.GetNames(typeof(PaymentMethods)).ToList();
				string response = JsonSerializer.Serialize(paymentMethods);
				return Ok(response);
			}
			catch (Exception)
			{
				return NotFound();
			}
		}

		[HttpGet]
		[Route("categories")]
		public async Task<IActionResult> GetCategories()
		{
			try
			{
				IList<Category> categories = await _categoriesRepo.ReadAll();
				string response = JsonSerializer.Serialize(categories);
				return Ok(response);
			}
			catch (Exception)
			{
				return NotFound();
			}
		}
	}
}
