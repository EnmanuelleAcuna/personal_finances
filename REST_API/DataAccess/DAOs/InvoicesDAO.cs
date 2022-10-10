using Microsoft.EntityFrameworkCore;
using REST_API.DataAccess.EntityFramework;
using REST_API.Extras;
using REST_API.Models;

namespace REST_API.DataAccess;

public class InvoicesDAO : ICRUDBase<Invoice>
{
	private readonly InvoicesDBContext _context;

	public InvoicesDAO(InvoicesDBContext context)
	{
		_context = context;
	}

	public async Task<IList<Invoice>> ReadAll()
	{
		if (_context.Invoices == null) throw new NullReferenceException("Entity set 'InvoicesDBContext.Invoices'  is null.");

		return await _context.Invoices.Include(i => i.CategoryNavigation)
									  .OrderByDescending(i => i.InvoiceDate)
									  .Select(i => ToDomainModel(i))
									  .ToListAsync();
	}

	public async Task<Invoice> ReadById(Guid id)
	{
		if (_context.Invoices == null) throw new NullReferenceException("Entity set 'InvoicesDBContext.Invoices'  is null.");

		Invoices? invoiceDB = await _context.Invoices.Include(i => i.CategoryNavigation)
													 .Where(i => i.Id == id)
													 .FirstOrDefaultAsync();

		if (invoiceDB == null) throw new NullReferenceException("Invoice not found.");

		return ToDomainModel(invoiceDB);
	}

	public async Task Create(Invoice invoice)
	{
		if (_context.Invoices == null) throw new NullReferenceException("Entity set 'InvoicesDBContext.Invoices'  is null.");

		Invoices? invoiceDB = ToDBModel(invoice);

		_context.Invoices.Add(invoiceDB);
		_context.Entry(invoiceDB).State = EntityState.Added;

		await _context.SaveChangesAsync();
	}

	public async Task Update(Invoice invoice)
	{
		try
		{
			Invoices? invoiceDB = await _context.Invoices.FindAsync(invoice.Id);

			if (invoiceDB == null) throw new NullReferenceException("Invoice not found.");

			invoiceDB.InvoiceDate = invoice.Date;
			invoiceDB.Amount = invoice.Amount;
			invoiceDB.PaymentMethod = invoice.GetPaymentMethod().ToString();
			invoiceDB.Payee = String.IsNullOrEmpty(invoice.Payee) ? String.Empty : invoice.Payee;
			invoiceDB.Detail = invoice.Detail;
			invoiceDB.Category = invoice.Category.Id;

			_context.Invoices.Update(invoiceDB);
			_context.Entry(invoiceDB).State = EntityState.Modified;

			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException) when (!InvoiceExists(invoice.Id.ToString()))
		{
			throw new NullReferenceException("Invoice already exists.");
		}
	}

	public async Task Delete(Guid id)
	{
		if (_context.Invoices == null) throw new NullReferenceException("Entity set 'InvoicesDBContext.Invoices'  is null.");

		Invoices? invoiceDB = await _context.Invoices.FindAsync(id);

		if (invoiceDB == null) throw new NullReferenceException("Invoice not found.");

		_context.Invoices.Remove(invoiceDB);
		_context.Entry(invoiceDB).State = EntityState.Deleted;

		await _context.SaveChangesAsync();
	}

	public static Invoice ToDomainModel(Invoices invoiceDB)
	{
		PaymentMethods paymentMethod = invoiceDB.PaymentMethod.ToPaymentMethod();
		Invoice invoice = Factories.GetInvoice(paymentMethod, invoiceDB.Id);
		invoice.FillInvoice(invoiceDB.InvoiceDate, invoiceDB.Amount, invoiceDB.Payee, invoiceDB.Detail, CategoriesDAO.ToDomainModel(invoiceDB.CategoryNavigation));
		return invoice;
	}

	public static Invoices ToDBModel(Invoice invoice)
	{
		Invoices invoiceDB = new Invoices
		{
			Id = invoice.Id,
			InvoiceDate = invoice.Date,
			Amount = invoice.Amount,
			PaymentMethod = invoice.GetPaymentMethod().ToString(),
			Payee = String.IsNullOrEmpty(invoice.Payee) ? String.Empty : invoice.Payee,
			Detail = invoice.Detail,
			Category = invoice.Category.Id
		};

		return invoiceDB;
	}

	private bool InvoiceExists(string id)
	{
		return (_context.Invoices?.Any(e => e.Id.Equals(new Guid(id)))).GetValueOrDefault();
	}
}
