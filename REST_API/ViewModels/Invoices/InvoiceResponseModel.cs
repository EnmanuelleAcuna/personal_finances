using REST_API.Extras;

namespace REST_API.Models;

public class InvoiceResponseModel
{
	public InvoiceResponseModel(Invoice invoice)
	{
		Id = invoice.Id.ToString();
		PaymentMethod = invoice.GetPaymentMethod().GetName();
		Date = invoice.Date;
		Amount = invoice.Amount;
		Payee = String.IsNullOrEmpty(invoice.Payee) ? String.Empty : invoice.Payee;
		CategoryName = String.IsNullOrEmpty(invoice.Category?.Name) ? String.Empty : invoice.Category.Name;
	}

	public string Id { get; }
	public string PaymentMethod { get; }
	public DateTime Date { get; }
	public decimal Amount { get; }
	public string Payee { get; }
	public string CategoryName { get; }
}
