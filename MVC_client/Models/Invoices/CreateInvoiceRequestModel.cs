using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Client.Models;

public class CreateInvoiceRequestModel
{
	[Display(Name = "Payment method")]
	[Required(ErrorMessage = "Please select a payment method.")]
	public string? PaymentMethod { get; set; }

	[Display(Name = "Category")]
	[Required(ErrorMessage = "Please select a category.")]
	public string? Category { get; set; }

	[Required(ErrorMessage = "Please enter a date")]
	[RegularExpression("^\\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Please enter a valid date.")]
	// [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
	public string? Date { get; set; }

	[Required(ErrorMessage = "Please specify an amount.")]
	[RegularExpression("^\\d{1,11}$|(?=^.{1,11}$)^\\d+\\.\\d{0,2}$", ErrorMessage = "Please enter a valid amount.")]
	[DataType(DataType.Currency, ErrorMessage = "Please enter a valid amount.")]
	public string? Amount { get; set; }

	[Required(ErrorMessage = "Please enter a payee.")]
	[MinLength(5, ErrorMessage = "The payee name must be at least 5 characters long.")]
	public string Payee { get; set; } = String.Empty;

	public string? Detail { get; set; }

	public string? CategoryId { get; set; }

	public string Secret { get; set; } = "5a16b6b7-cddb-4752-9eb0-54abf2d43a68";
}