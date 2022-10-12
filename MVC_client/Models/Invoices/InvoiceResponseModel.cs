using System.ComponentModel.DataAnnotations;

namespace MVC_Client.Models;

public class InvoiceResponseModel
{
	public string Id { get; set; } = string.Empty;

	[Display(Name = "Payment method")]
	public string PaymentMethod { get; set; } = string.Empty;

	public string Category { get; set; } = string.Empty;
	public DateTime Date { get; set; }
	public decimal Amount { get; set; }
	public string Payee { get; set; } = string.Empty;

	[Display(Name = "Category")]
	public string CategoryName { get; set; } = string.Empty;

	public string DateFormatted
	{
		get
		{
			return Date.ToString("dd/MM/yyyy");
		}
	}

	public string AmountFormatted
	{
		get
		{
			return String.Format("{0:###,##0.00}", Amount);
		}
	}
}