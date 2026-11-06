using Claims.Auditing;
using Claims.Models;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;


namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ICoversService _coversService;
        private readonly ILogger<CoversController> _logger;
        private readonly IAuditer _auditer;

        public CoversController(ICoversService coversService, IAuditer auditer, ILogger<CoversController> logger)
        {
            _coversService = coversService;
            _logger = logger;
            _auditer = auditer;
        }

        [HttpPost("compute")]
        public ActionResult ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
        {
            return Ok(_coversService.ComputePremium(startDate, endDate, coverType));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cover>>> GetAsync()
        {
            var results = await _coversService.GetCoversAsync();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cover>> GetAsync(string id)
        {
            var cover = await _coversService.GetCoverAsync(id);
            if (cover is null)
            {
                return NotFound();
            }
            return Ok(cover);
        }


        [HttpPost]
        public async Task<ActionResult> CreateAsync(Cover cover)
        {
            try
            {
                var createdCover = await _coversService.CreateCoverAsync(cover);

                await _auditer.AuditCoverAsync(cover.Id, "POST");

                return Ok(createdCover);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var cover = await _coversService.GetCoverAsync(id);

            if (cover is null)
            {
                return NotFound();
            }

            await _coversService.DeleteCoverAsync(id);
            await _auditer.AuditCoverAsync(id, "DELETE");

            return NoContent();
        }
    }
}
