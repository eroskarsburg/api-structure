using Bank.API.OutputPort;
using Bank.Application.Payments.v1;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controller.V1;

[ApiController]
[Route("v1/[controller]")]
public sealed class PaymentsController(
    IOutputPort<PaymentResponse> outputPort,
    IPaymentService paymentService)
    : ControllerBase
{
    [HttpPost("refund")]
    public async Task<IActionResult> Refund([FromBody] PaymentRequest request, CancellationToken cancellationToken)
    {
        var result = await paymentService.ProcessAsync(request, cancellationToken);
        return outputPort.Response(result);
    }
}
