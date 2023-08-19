using System;
using System.Collections.Generic;

namespace REST_API.DataAccess.EntityFramework
{
    public partial class Invoices
    {
        public Guid Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string Payee { get; set; } = null!;
        public string? Detail { get; set; }
        public Guid Category { get; set; }

        public virtual Categories CategoryNavigation { get; set; } = null!;
    }
}
