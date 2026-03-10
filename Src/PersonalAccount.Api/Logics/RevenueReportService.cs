using System;
using System.Security.Cryptography.X509Certificates;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Реализация интерфейса <see cref="IRevenueReportService"/>
/// </summary>
public class RevenueReportService : IRevenueReportService
{
    /// <summary>
    /// Сформировать отчет
    /// </summary>
    /// <param name="transactions"> Набор транзакций. </param>
    /// <returns></returns>
    public Task<IEnumerable<RevenueDto>> Create(IEnumerable<TransactionModel> transactions)
    {
        if(!transactions.Any()) return Task.FromResult(  Enumerable.Empty<RevenueDto>() );

        // Все скидки
        var allDiscount = transactions
                            .Where(x => x.Type == Domain.Core.TransactionType.RefundPayment)
                            .GroupBy(x => x.Period.Date)
                            .Select(x => new {
                                Key  = x.Key,
                                Value = x.Sum(t => t.Price * t.Quantuty)
                            })
                            .ToDictionary(x => x.Key, x => x.Value);

        // Рассчитать все банковские оплаты
        var calcBankTask = Task.Run( () =>
        {
            var allPayments = transactions
                            .Where(x => x.Type == Domain.Core.TransactionType.BankPayment)
                            .GroupBy(x => x.Period.Date) 
                            .Select(x => new {
                                Key  = x.Key,
                                Value = x.Sum(t => t.Price * t.Quantuty)
                            })
                            .ToDictionary(x => x.Key, x => x.Value);

            return allPayments.ToDictionary(
                pair => pair.Key,
                pair => pair.Value
                        - (allDiscount.ContainsKey(pair.Key) ?  pair.Value : 0)
            );
        });

        // Рассчитать все оплаты наличными
        var calcCashTask = Task.Run( () =>
        {
             var allPayments = transactions
                            .Where(x => x.Type == Domain.Core.TransactionType.BankPayment)
                            .GroupBy(x => x.Period.Date) 
                            .Select(x => new {
                                Key  = x.Key,
                                Value = x.Sum(t => t.Price * t.Quantuty)
                            })
                            .ToDictionary(x => x.Key, x => x.Value);


            var allRefunds = transactions
                            .Where(x => x.Type == Domain.Core.TransactionType.RefundPayment)
                              .GroupBy(x => x.Period.Date)
                            .Select(x => new {
                                Key  = x.Key,
                                Value = x.Sum(t => t.Price * t.Quantuty)
                            })
                            .ToDictionary(x => x.Key, x => x.Value);

            return allPayments.ToDictionary(
                pair => pair.Key,
                pair => pair.Value 
                        - (allDiscount.ContainsKey(pair.Key) ?  allDiscount[pair.Key]  : 0)
                        - (allRefunds.ContainsKey(pair.Key) ? allRefunds[pair.Key] : 0)
                
            );                
        });

        // Ожидаем расчета
        Task.WaitAll( calcBankTask, calcCashTask);

        // Получим список всех дат
        var periods = calcBankTask.Result.Keys
                    .Union( calcCashTask.Result.Keys )
                    .Union( allDiscount.Keys )
                    .Distinct()
                    .ToList();

        // Формируем результат
        var result = periods.Select( x => new RevenueDto()
        {
            Period = x,
            BankAmount = calcBankTask.Result.ContainsKey( x ) ? calcBankTask.Result[ x ] : 0,
            CashAmount = calcCashTask.Result.ContainsKey( x ) ? calcCashTask.Result[ x ] : 0,
            DiscountAmount = allDiscount.ContainsKey( x ) ? allDiscount[ x ] : 0,
            Owner = transactions.FirstOrDefault()?.Owner.Id ?? Guid.Empty
        });

        return Task.FromResult ( result );            
    }
}
