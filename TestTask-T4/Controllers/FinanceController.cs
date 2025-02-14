using Microsoft.AspNetCore.Mvc;
using TestTask_T4.Contracts;
using TestTask_T4.Model;

namespace TestTask_T4.Controllers;

[Route("/")]
public class FinanceController : ControllerBase
{
    private readonly ILogger<FinanceController> _logger;

    public FinanceController(ILogger<FinanceController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("balance")]
    public Client GetBalance([FromQuery]Guid id)
    {
        return new Client();
    }

    [HttpPost]
    [Route("credit")]
    public TransactionRespose Credit([FromBody] CreditTransactionRequest request)
    {
        return new TransactionRespose();
    }

    [HttpPost]
    [Route("debit")]
    public TransactionRespose Credit([FromBody] DebitTransactionRequest request)
    {
        return new TransactionRespose();
    }

    [HttpPost]
    [Route("revert")]
    public RevertTransactionResponse Revert([FromQuery] Guid id)
    {
        return new RevertTransactionResponse();
    }
}
