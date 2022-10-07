using System.Text.Json;
using REST_API.Models;

namespace REST_API.Extras;

public static class Extensions
{
	public static string ToMyString(this ToDoItem toDoItem)
	{
		return JsonSerializer.Serialize(toDoItem);
	}

	public static string ToString(this PaymentMethods paymentMethod)
	{
		switch (paymentMethod)
		{
			case PaymentMethods.Cash:
				return "Cash";
			case PaymentMethods.DebitCard:
				return "DebitCard";
			case PaymentMethods.Transfer:
				return "Transfer";
			default:
				return string.Empty;
		}
	}

	public static PaymentMethods ToPaymentMethod(this string paymentMethod)
	{
		return (PaymentMethods)Enum.Parse(typeof(PaymentMethods), paymentMethod);

		// switch (paymentMethod.ToLower())
		// {
		// 	case "cash":
		// 		return PaymentMethod.Cash;
		// 	case "debitcard":
		// 		return PaymentMethod.DebitCard;
		// 	case "transfer":
		// 		return PaymentMethod.Transfer;
		// 	default:
		// 		return PaymentMethod.Cash;
		// }
	}

	public static PaymentMethods GetPaymentMethod(this Invoice invoice)
	{
		PaymentMethods paymentMethod = PaymentMethods.Cash;

		if (invoice is DebitCardPaidInvoice) paymentMethod = PaymentMethods.DebitCard;
		if (invoice is TransferPaidInvoice) paymentMethod = PaymentMethods.Transfer;

		return paymentMethod;
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
