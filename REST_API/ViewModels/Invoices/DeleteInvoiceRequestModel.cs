using REST_API.DataAccess;
using REST_API.Extras;

namespace REST_API.Models;

public class DeleteInvoiceRequestModel
{
	public string? Id { get; set; }
	public string? Secret { get; set; }

	public async Task<Invoice> ToDomainModel(ICRUDBase<Invoice> invoicesRepo)
	{
		if (String.IsNullOrEmpty(Id)) throw new NullReferenceException(message: nameof(Id));
		if (String.IsNullOrEmpty(Secret) || !new Guid(Secret).IsValid()) throw new NullReferenceException(message: nameof(Secret));

		Invoice invoice = await invoicesRepo.ReadById(new Guid(Id));
		return invoice;
	}
}
