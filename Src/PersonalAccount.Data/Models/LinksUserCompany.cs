using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class LinksUserCompany
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? BranchId { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual User? User { get; set; }
}
