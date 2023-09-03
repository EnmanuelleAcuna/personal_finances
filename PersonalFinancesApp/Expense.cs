using System;
using System.Text.Json;

namespace PersonalFinances;

class Expense
{
	public Expense(Guid id,
				   PaymentMethods paymentMethod,
				   DateTime transactionDate,
				   decimal amount,
				   string payee,
				   string detail,
				   Categories category)
	{
		Id = id;
		PaymentMethod = paymentMethod;
		Date = transactionDate;
		Amount = amount;
		Payee = payee;
		Detail = detail;
		Category = category;
	}

	public Guid Id { get; }
	public PaymentMethods PaymentMethod { get; }
	public DateTime Date { get; }
	public decimal Amount { get; }
	public string Payee { get; }
	public string Detail { get; }
	public Categories Category { get; }

	public override string ToString() => JsonSerializer.Serialize(this);
}
