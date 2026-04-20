using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель филиала в составе организации.
/// </summary>
public class BranchModel : DomainModel
{
    /// <summary>
    /// Организация-владелец.
    /// </summary>
    [Required]
    public CompanyModel Company { get; set; } = null!;

    /// <summary>
    /// Наименование филиала.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
}
