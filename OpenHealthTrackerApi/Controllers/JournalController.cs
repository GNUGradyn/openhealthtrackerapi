using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenHealthTrackerApi.Controllers;

[Route("journal")]
public class JournalController
{
    [HttpGet, HttpPost]
    [Route("entry")]
    [Authorize]
    public async Task<IActionResult> CreateEntry()
    {
        return new ContentResult() { StatusCode = 204 };
    }
}