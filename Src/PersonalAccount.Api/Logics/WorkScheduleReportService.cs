using System;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Реализация интерфейса <see cref="IWorkScheduleReportService"/>
/// </summary>
public class WorkScheduleReportService : IWorkScheduleReportService
{
    /// <summary>
    /// Сформировать отчет
    /// </summary>
    /// <param name="transactions"> Набор транзакций. </param>
    /// <returns></returns>
    public IEnumerable<WorkScheduleDto> Create(IEnumerable<TransactionModel> transactions)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Реализация ассинхронного варианта
    /// </summary>
    /// <param name="transactions"> Набор транзакций. </param>
    /// <returns></returns>
    public async Task<IEnumerable<WorkScheduleDto>> CreateAsync(IEnumerable<TransactionModel> transactions, CancellationToken token)
         => await Task.Run( () => Create( transactions), token);
}
