using Bank.API.OutputPort;
using Bank.Application.Payments;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controllers;

[ApiController]
[Route("api/v1/qrcodes")]
public class QrCodeController(
    IQrCodeService qrCodeService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GenerateQrCode([FromBody] QrCodeGenerationRequest request, CancellationToken cancellationToken)
    {
        var result = await qrCodeService.GenerateAsync(request, cancellationToken);
        var presenter = new OutPutPortPresenter<QrCodeGenerationResponse>(this);
        return presenter.Responder(result);
    }

    [HttpGet("{qrCodeId:guid}")]
    public async Task<IActionResult> GetQrCodeById([FromRoute] Guid qrCodeId, CancellationToken cancellationToken)
    {
        var result = await qrCodeService.GetByIdAsync(qrCodeId, cancellationToken);
        var presenter = new OutPutPortPresenter<QrCodeGenerationResponse>(this);
        return presenter.Responder(result);
    }
}
