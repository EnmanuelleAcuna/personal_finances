using System;
using System.Collections.Generic;

namespace REST_API.DataAccess.EntityFramework
{
    public partial class Categories
    {
        public Categories()
        {
            Invoices = new HashSet<Invoices>();
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Invoices> Invoices { get; set; }
    }
}
