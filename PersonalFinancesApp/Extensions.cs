using System;

namespace PersonalFinances;

internal static class Extensions
{
	public static string Name(this PaymentMethods paymentMethod)
	{
		return paymentMethod switch
		{
			PaymentMethods.Cash => "Cash",
			PaymentMethods.DebitCard => "DebitCard",
			PaymentMethods.CreditCard => "CreditCard",
			PaymentMethods.Transfer => "Transfer",
			_ => string.Empty
		};
	}

	internal static PaymentMethods ToPaymentMethod(this string paymentMethod)
	{
		// return (PaymentMethods)Enum.Parse(typeof(PaymentMethods), paymentMethod);

		return paymentMethod.ToLower() switch
		{
			"cash" => PaymentMethods.Cash,
			"debitcard" or "debit card" => PaymentMethods.DebitCard,
			"creditcard" or "credit card" => PaymentMethods.CreditCard,
			"transfer" => PaymentMethods.Transfer,
			_ => throw new ArgumentException(message: "Invalid payment method.", paramName: nameof(paymentMethod))
		};
	}

	public static DateTime ToDateTime(this string dateTime)
	{
		// Expected format: 28/05/2022
		int day = Convert.ToInt16(dateTime[..2]);
		int month = Convert.ToInt16(dateTime.Substring(3, 2));
		int year = Convert.ToInt16(dateTime.Substring(6, 4));
		return new DateTime(year, month, day);
	}

	public static Guid ToGuid(this string text)
	{
		if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)) return new Guid();
		return new Guid(text);
	}
}
