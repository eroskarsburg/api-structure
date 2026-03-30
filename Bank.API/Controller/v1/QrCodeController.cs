using Bank.API.OutputPort;
using Bank.Application.QrCode.v1;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controller.V1;

[ApiController]
[Route("v1/[controller]")]
public sealed class QrCodeController(
    IQrCodeService qrCodeService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GenerateQrCode([FromBody] QrCodeRequest request, CancellationToken cancellationToken)
    {
        var result = await qrCodeService.GenerateAsync(request, cancellationToken);
        var presenter = new OutputPort<QrCodeResponse>(this);
        return presenter.Response(result);
    }

    [HttpGet("{qrCodeId:guid}")]
    public async Task<IActionResult> GetQrCodeById([FromRoute] Guid qrCodeId, CancellationToken cancellationToken)
    {
        var result = await qrCodeService.GetByIdAsync(qrCodeId, cancellationToken);
        var presenter = new OutputPort<QrCodeResponse>(this);
        return presenter.Response(result);
    }
}
