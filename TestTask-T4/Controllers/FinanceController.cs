using Microsoft.AspNetCore.Mvc;
using TestTask_T4.Contracts;
using TestTask_T4.Mappers;
using TestTask_T4.Model;
using TestTask_T4.Services.Clients;
using TestTask_T4.Services.Finance;

namespace TestTask_T4.Controllers;

[ApiController]
[Route("/")]
public class FinanceController : ControllerBase
{
    private readonly ILogger<FinanceController> _logger;
    private readonly IFinanceService _financeService;
    private readonly IClientsService _clientsService;

    public FinanceController(ILogger<FinanceController> logger, IFinanceService financeService, IClientsService clientsService)
    {
        _logger = logger;
        _financeService = financeService;
        _clientsService = clientsService;
    }

    [HttpGet]
    [Route("balance")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")]
    public async Task<ClientBalanceResponse> GetBalance([FromQuery]Guid id, CancellationToken cancellationToken)
    {
        var clientBalance = await _clientsService.GetClientBalance(id, cancellationToken);
        return clientBalance.ToResponse();
    }

    [HttpPost]
    [Route("credit")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")]
    public async Task<TransactionRespose> Credit([FromBody] CreditTransactionRequest request, CancellationToken cancellationToken)
    {
        var transaction = request.ToTransaction();

        var result = await _financeService.ProcessTransaction(transaction, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost]
    [Route("debit")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")]
    public async Task<TransactionRespose> Credit([FromBody] DebitTransactionRequest request, CancellationToken cancellationToken)
    {
        var transaction = request.ToTransaction();

        var result = await _financeService.ProcessTransaction(transaction, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost]
    [Route("revert")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")]
    public async Task<RevertTransactionResponse> Revert([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var result = await _financeService.RevertTransaction(id, cancellationToken);

        return result.ToResponse();
    }
}
