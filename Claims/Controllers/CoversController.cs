using Claims.Auditing;
using Claims.Models;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ILogger<ClaimsController> _logger;
    private readonly IClaimsService _claimsService;
    private readonly IAuditer _auditer;

    public ClaimsController(ILogger<ClaimsController> logger, IClaimsService claimsService, IAuditer auditer)
    {
        _logger = logger;
        _claimsService = claimsService;
        _auditer = auditer;
    }

    [HttpGet]
    public async Task<IEnumerable<Claim>> GetAsync()
    {
        return await _claimsService.GetClaimsAsync();
    }


    [HttpPost]
    public async Task<ActionResult> CreateAsync(Claim claim)
    {
        try
        {
            await _claimsService.AddItemAsync(claim);
            await _auditer.AuditClaimAsync(claim.Id, "POST");

            return Ok(claim);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var claim = await _claimsService.GetClaimAsync(id);

        if (claim is null)
        {
            return NotFound();
        }

        await _claimsService.DeleteItemAsync(id);
        await _auditer.AuditClaimAsync(id, "DELETE");

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Claim?>> GetAsync(string id)
    {
        var claim = await _claimsService.GetClaimAsync(id);

        if (claim is null)
        {
            return NotFound();
        }

        return Ok(claim);
    }
}

