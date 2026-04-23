using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class Branch
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? LoadOptions { get; set; }

    public bool IsDefault { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Emploee> Emploees { get; set; } = new List<Emploee>();

    public virtual ICollection<LinksUserCompany> LinksUserCompanies { get; set; } = new List<LinksUserCompany>();
}
