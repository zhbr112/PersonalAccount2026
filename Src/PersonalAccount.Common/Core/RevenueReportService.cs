using System;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Сервис для построения отчета "Выручка"
/// </summary>
public interface RevenueReportService : IRepository<RevenueDto>
{
    /// <summary>
    /// Сформировать отчет
    /// </summary>
    /// <param name="transactions"> Исходный набор транзакций </param>
    /// <returns></returns>
    public IEnumerable<RevenueDto> Create(IEnumerable<Transaction> transactions);
}
