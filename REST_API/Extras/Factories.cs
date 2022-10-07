using REST_API.Models;

namespace REST_API.Extras;

public static class Factories
{
	public static Invoice GetNewInvoice(PaymentMethods paymentMethod)
	{
		switch (paymentMethod)
		{
			case PaymentMethods.Transfer:
				return new TransferPaidInvoice(Guid.NewGuid());
			case PaymentMethods.DebitCard:
				return new DebitCardPaidInvoice(Guid.NewGuid());
			case PaymentMethods.Cash:
				return new CashPaidInvoice(Guid.NewGuid());
			default:
				return new CashPaidInvoice(Guid.NewGuid());
		}
	}

	public static Invoice GetInvoice(PaymentMethods paymentMethod, Guid id)
	{
		switch (paymentMethod)
		{
			case PaymentMethods.Transfer:
				return new TransferPaidInvoice(id);
			case PaymentMethods.DebitCard:
				return new DebitCardPaidInvoice(id);
			case PaymentMethods.Cash:
				return new CashPaidInvoice(id);
			default:
				return new CashPaidInvoice(id);
		}
	}
}
