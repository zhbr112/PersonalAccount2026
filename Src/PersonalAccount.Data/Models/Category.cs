using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid? BranchId { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual ICollection<Nomenclature> Nomenclatures { get; set; } = new List<Nomenclature>();
}
