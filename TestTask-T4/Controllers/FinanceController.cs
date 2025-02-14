using Microsoft.AspNetCore.Mvc;
using TestTask_T4.Contracts;
using TestTask_T4.Mappers;
using TestTask_T4.Model;
using TestTask_T4.Services.Finance;

namespace TestTask_T4.Controllers;

[Route("/")]
public class FinanceController : ControllerBase
{
    private readonly ILogger<FinanceController> _logger;
    private readonly IFinanceService _financeService;

    public FinanceController(ILogger<FinanceController> logger, IFinanceService financeService)
    {
        _logger = logger;
        _financeService = financeService;
    }

    [HttpGet]
    [Route("balance")]
    public async Task<Client> GetBalance([FromQuery]Guid id)
    {
        return new Client();
    }

    [HttpPost]
    [Route("credit")]
    public async Task<TransactionRespose> Credit([FromBody] CreditTransactionRequest request)
    {
        var transaction = request.ToTransaction();

        var result = await _financeService.ProcessTransaction(transaction);

        return result.ToResponse();
    }

    [HttpPost]
    [Route("debit")]
    public async Task<TransactionRespose> Credit([FromBody] DebitTransactionRequest request)
    {
        var transaction = request.ToTransaction();

        var result = await _financeService.ProcessTransaction(transaction);

        return result.ToResponse();
    }

    [HttpPost]
    [Route("revert")]
    public async Task<RevertTransactionResponse> Revert([FromQuery] Guid id)
    {
        return new RevertTransactionResponse();
    }
}
