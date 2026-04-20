using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Controllers
{
    /// <summary>
    /// Контроллер для вызов Api (загрузка данных)
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ConsoleController : ControllerBase
    {
        // Сервис загрузки данных
        private readonly ILoadingService _loadingService;

        public ConsoleController(ILoadingService loadingService) => _loadingService = loadingService;

        /// <summary>
        /// Выполнить загрузку данных в raw таблицу из журнала.
        /// </summary>
        /// <param name="branchId"> Уникальный код филиала </param>
        /// <param name="transactions"> Список транзакций </param>
        /// <returns></returns>
        [HttpPost("branch/{branchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PushToBranch(
            [FromRoute]
            Guid branchId,
            [FromBody]
            IEnumerable<JournalRowDto> transactions,
            CancellationToken token
        )
        {
            var result = await _loadingService.PushAsync(branchId, transactions, token);
            if(result) return Ok();
            else
                return BadRequest();
        }

        // Пример запроса.
        // http://0.0.0.0:8000/console/14e54725-0efc-42b8-a27d-a84f9a7257c5

        /// <summary>
        /// Получить текущие настройки филиала
        /// </summary>
        /// <param name="branchId"> Уникальный код филиала </param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("branch/{branchId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoadingSettingsModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetBranchSettings(
            [FromRoute]
            Guid branchId,
             CancellationToken token
        )
        {
            var result = await _loadingService.GetSettingsAsync(branchId, token);
            if(result is not null) return Ok(result);
            else
                return BadRequest();
        }

    }
}
