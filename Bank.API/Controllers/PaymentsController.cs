using Bank.API.OutputPort;
using Bank.Application.Payments;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(
    IPaymentService _paymentService)
    : ControllerBase
{
    [HttpPost("v1/refund")]
    public async Task<IActionResult> Refund([FromBody] PaymentRequest request, CancellationToken cancellationToken)
    {
        var result = await _paymentService.ProcessAsync(request, cancellationToken);
        var presenter = new OutPutPortPresenter<PaymentResponse>(this);
        return presenter.Responder(result);
    }

}
