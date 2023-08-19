namespace REST_API.Models;

public class CashPaidInvoice : Invoice
{
	public CashPaidInvoice(Guid id) : base(id) { }
}

public class DebitCardPaidInvoice : Invoice
{
	public DebitCardPaidInvoice(Guid id) : base(id) { }
}

public class CreditCardPaidInvoice : Invoice
{
	public CreditCardPaidInvoice(Guid id) : base(id) { }
}

public class TransferPaidInvoice : Invoice
{
	public TransferPaidInvoice(Guid id) : base(id) { }
}

public class Invoice
{
	protected Invoice(Guid id)
	{
		Id = id;
		Category = new Category(Guid.NewGuid(), String.Empty);
	}

	public Guid Id { get; protected set; }
	public DateTime Date { get; protected set; }
	public decimal Amount { get; protected set; }
	public string? Payee { get; protected set; }
	public string? Detail { get; protected set; }
	public Category Category { get; private set; }

	public void FillInvoice(DateTime date, decimal amount, string payee, string? detail, Category category)
	{
		Date = date;
		Amount = amount;
		Payee = payee;
		Detail = detail;
		Category = category;
	}
}
