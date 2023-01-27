using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenHealthTrackerApi.Controllers;

[Route("journal")]
[Authorize]
public class JournalController
{
    [HttpGet, HttpPost]
    [Route("entry")]
    public async Task<IActionResult> CreateEntry()
    {
        return new ContentResult() { StatusCode = 204 };
    }
}