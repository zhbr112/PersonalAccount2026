namespace PersonalAccount.Data.Models;

public partial class Company
{
    public Guid Id { get; set; }

    public string? Inn { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

}
