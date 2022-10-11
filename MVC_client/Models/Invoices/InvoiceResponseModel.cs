using System.ComponentModel.DataAnnotations;

namespace MVC_Client.Models;

public class InvoiceResponseModel
{
	public string Id { get; set; } = string.Empty;

	[Display(Name = "Payment method")]
	public string PaymentMethod { get; set; } = string.Empty;

	public string Category { get; set; } = string.Empty;

	public string Date { get; set; } = string.Empty;

	public decimal Amount { get; set; }

	public string Payee { get; set; } = string.Empty;

	public string Detail { get; set; } = string.Empty;
}