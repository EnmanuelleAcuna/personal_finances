using System.Text.Json;
using REST_API.Models;

namespace REST_API.Extras;

public static class Extensions
{
	public static string ToJsonString(this ToDoItem toDoItem)
	{
		return JsonSerializer.Serialize(toDoItem);
	}

	public static string GetName(this PaymentMethods paymentMethod)
	{
		switch (paymentMethod)
		{
			case PaymentMethods.Cash:
				return "Cash";
			case PaymentMethods.DebitCard:
				return "Debit card";
			case PaymentMethods.CreditCard:
				return "Credit card";
			case PaymentMethods.Transfer:
				return "Transfer";
			default:
				return string.Empty;
		}
	}

	public static PaymentMethods ToPaymentMethod(this string paymentMethod)
	{
		// return (PaymentMethods)Enum.Parse(typeof(PaymentMethods), paymentMethod);

		switch (paymentMethod.ToLower())
		{
			case "cash":
				return PaymentMethods.Cash;
			case "debitcard":
			case "debit card":
				return PaymentMethods.DebitCard;
			case "creditcard":
			case "credit card":
				return PaymentMethods.CreditCard;
			case "transfer":
				return PaymentMethods.Transfer;
			default:
				throw new ArgumentException(message: "Invalid payment method.", paramName: nameof(paymentMethod));
		}
	}

	public static Dictionary<string, string> GetPaymentMethodsList()
	{
		Dictionary<string, string> paymentMethods = new Dictionary<string, string>();

		paymentMethods.Add("cash", "Cash");
		paymentMethods.Add("debitcard", "Debit card");
		paymentMethods.Add("creditcard", "Credit card");
		paymentMethods.Add("transfer", "Transfer");

		return paymentMethods;
	}

	public static PaymentMethods GetPaymentMethod(this Invoice invoice)
	{
		if (invoice is CashPaidInvoice) return PaymentMethods.Cash;
		if (invoice is DebitCardPaidInvoice) return PaymentMethods.DebitCard;
		if (invoice is CreditCardPaidInvoice) return PaymentMethods.CreditCard;
		if (invoice is TransferPaidInvoice) return PaymentMethods.Transfer;

		throw new ArgumentException(message: "Invalid payment method.");
	}

	public static DateTime ToDateTime(this string dateTime)
	{
		// Expected format: 28/05/2022
		int day = Convert.ToInt16(dateTime.Substring(0, 2));
		int month = Convert.ToInt16(dateTime.Substring(3, 2));
		int year = Convert.ToInt16(dateTime.Substring(6, 4));
		return new DateTime(year, month, day);
	}

	public static Guid ToGuid(this string text)
	{
		if (string.IsNullOrEmpty(text)) return new Guid();
		if (string.IsNullOrWhiteSpace(text)) return new Guid();
		Guid guid = new Guid(text);
		return guid;
	}

	public static bool IsValid(this Guid secret)
	{
		if (secret.Equals(Guid.Empty)) return false;
		if (!Guid.TryParse(secret.ToString(), out Guid idObjeto)) return false;
		if (secret.Equals(new Guid())) return false;
		Guid uuid = new Guid("5a16b6b7-cddb-4752-9eb0-54abf2d43a68");
		if (secret.Equals(uuid)) return true;
		return false;
	}
}
