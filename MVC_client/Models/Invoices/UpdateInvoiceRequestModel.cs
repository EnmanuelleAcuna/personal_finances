namespace MVC_Client.Models;

public class UpdateInvoiceRequestModel
{
	public string Id { get; set; } = String.Empty;
	public string PaymentMethod { get; set; } = String.Empty;
	public string Date { get; set; } = String.Empty;
	public decimal Amount { get; set; }
	public string Payee { get; set; } = String.Empty;
	public string Detail { get; set; } = String.Empty;
	public string Secret { get; set; } = String.Empty;
}