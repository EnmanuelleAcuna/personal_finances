using REST_API.DataAccess;
using REST_API.Extras;

namespace REST_API.Models;

public class UpdateInvoiceRequestModel
{
	public string? Id { get; set; }
	public string? PaymentMethod { get; set; }
	public string? Date { get; set; }
	public decimal Amount { get; set; }
	public string? Payee { get; set; }
	public string? Detail { get; set; }
	public string? CategoryId { get; set; }
	public string? Secret { get; set; }

	public async Task<Invoice> ToDomainModel(ICRUDBase<Category> categoriesRepo)
	{
		if (String.IsNullOrEmpty(Id)) throw new NullReferenceException(message: nameof(Id));
		if (String.IsNullOrEmpty(PaymentMethod)) throw new NullReferenceException(message: nameof(PaymentMethod));
		if (String.IsNullOrEmpty(Date)) throw new NullReferenceException(message: nameof(Date));
		if (String.IsNullOrEmpty(Payee)) throw new NullReferenceException(message: nameof(Payee));
		if (String.IsNullOrEmpty(CategoryId)) throw new NullReferenceException(message: nameof(CategoryId));
		if (String.IsNullOrEmpty(Secret) || new Guid(Secret).IsValid()) throw new NullReferenceException(message: nameof(Secret));

		Invoice invoice = Factories.GetInvoice(PaymentMethod.ToPaymentMethod(), new Guid(Id));
		Category category = await categoriesRepo.ReadById(new Guid(CategoryId));
		invoice.FillInvoice(Date.ToDateTime(), Amount, Payee, Detail, category);
		return invoice;
	}
}
