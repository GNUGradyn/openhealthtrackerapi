using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenHealthTrackerApi.Data.Models;
using OpenHealthTrackerApi.Models;
using OpenHealthTrackerApi.Services.BLL;

namespace OpenHealthTrackerApi.Controllers;

[Route("journal")]
[Authorize]
public class JournalController : ControllerBase
{
    private readonly IJournalService _journalService;

    public JournalController(IJournalService journalService)
    {
        _journalService = journalService;
    }

    [HttpPost]
    [Route("entry")]
    public async Task<IActionResult> CreateEntry([FromBody] JournalEntryRequest request)
    {
        await _journalService.CreateEntry(request.Text, request.Emotions, request.Activities, getUserGuid());
        return StatusCode(204);
    }

    private Guid getUserGuid()
    {
        return new Guid(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
}