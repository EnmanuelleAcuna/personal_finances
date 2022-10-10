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
			IList<Invoice> invoices = await _invoicesRepo.ReadAll();
			IEnumerable<InvoiceResponseModel> response = invoices.Select(i => new InvoiceResponseModel(i));
			return Ok(response);
		}

		// GET: /invoices/jhds-ytrf-...
		[HttpGet("{id}")]
		public async Task<ActionResult<Invoice>> GetInvoiceById(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound();
			Invoice invoice = await _invoicesRepo.ReadById(new Guid(id));
			InvoiceResponseModel response = new InvoiceResponseModel(invoice);
			return Ok(response);
		}

		// POST: /invoices
		[HttpPost]
		public async Task<ActionResult<Invoice>> Create(CreateInvoiceRequestModel model)
		{
			Invoice invoice = await model.ToDomainModel(_categoriesRepo);
			await _invoicesRepo.Create(invoice);
			return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
		}

		// PUT: /invoices/jhds-ytrf-...
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(string id, UpdateInvoiceRequestModel model)
		{
			if (id != model.Id) throw new ArgumentException(message: "Invalid parameter.", paramName: nameof(id));
			Invoice invoice = await model.ToDomainModel(_categoriesRepo);
			await _invoicesRepo.Update(invoice);
			return NoContent();
		}

		// DELTE: /invoices
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id, DeleteInvoiceRequestModel model)
		{
			if (id != model.Id) throw new ArgumentException(message: "Invalid parameter.", paramName: nameof(id));
			Invoice invoice = await model.ToDomainModel(_invoicesRepo);
			if (invoice == null) return NotFound();
			await _invoicesRepo.Delete(invoice.Id);
			return NoContent();
		}

		// GET: /invoices/paymentmethods
		[HttpGet]
		[Route("paymentmethods")]
		public IActionResult GetPaymentMethods()
		{
			// IList<string> paymentMethods = Enum.GetNames(typeof(PaymentMethods)).ToList();
			string response = JsonSerializer.Serialize(Extensions.GetPaymentMethodsList());
			return Ok(response);
		}
	}
}

