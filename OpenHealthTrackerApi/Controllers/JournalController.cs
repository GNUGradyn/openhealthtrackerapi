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

    [HttpGet]
    [Route("entries")]
    public async Task<JsonResult> GetEntries([FromBody] PaginatedRequest request)
    {
        var results = await _journalService.GetEntriesAsync(request.ResultsPerPage, request.ResultsPerPage * (request.Page - 1), getUserGuid());
        return new JsonResult(results);
    }
    
    [HttpPost]
    [Route("entry")]
    public async Task<JsonResult> CreateEntry([FromBody] JournalEntryRequest request)
    {
        var result = await _journalService.CreateEntry(request.Text, request.Emotions, request.Activities, getUserGuid());
        return new JsonResult(new IdResponse(result));
    }

    private Guid getUserGuid()
    {
        return new Guid(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }

    [HttpGet]
    [Route("emotions")]
    public async Task<JsonResult> GetEmotions()
    {
        var results = await _journalService.GetEmotionsByUserAsync(getUserGuid());
        return new JsonResult(results);
    }

    [HttpPost]
    [Route("Emotions")]
    public async Task<JsonResult> CreateEmotion([FromBody] CreateEmotionRequest request)
    {
        var results = await _journalService.CreateEmotionAsync(request.Name, request.Category, getUserGuid());
        return new JsonResult(new IdResponse(results));
    }

    [HttpGet]
    [Route("activities")]
    public async Task<JsonResult> GetActivities()
    {
        var results = await _journalService.GetActivitiesByUserAsync(getUserGuid());
        return new JsonResult(results);
    }

    [HttpPost]
    [Route("activities")]
    public async Task<JsonResult> CreateActivity([FromBody] NamedObjectRequest request)
    {
        var result = await _journalService.CreateActivityAsync(request.Name, getUserGuid());
        return new JsonResult(new IdResponse(result));
    }
    
    [HttpGet]
    [Route("emotioncategories")]
    public async Task<JsonResult> GetEmotionCategories()
    {
        var results = await _journalService.GetEmotionCategoriesByUserAsync(getUserGuid());
        return new JsonResult(results);
    }

    [HttpPost]
    [Route("emotioncategories")]
    public async Task<JsonResult> CreateEmotionCategory([FromBody] NamedObjectRequest request)
    {
        var result = await _journalService.CreateEmotionCategoryAsync(request.Name, getUserGuid());
        return new JsonResult(new IdResponse(result));
    }
}