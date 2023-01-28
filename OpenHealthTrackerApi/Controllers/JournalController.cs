using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenHealthTrackerApi.Data.Models;
using OpenHealthTrackerApi.Models;
using OpenHealthTrackerApi.Pipeline;
using OpenHealthTrackerApi.Services.BLL;

namespace OpenHealthTrackerApi.Controllers;

[Route("journal")]
[Authorize]
public class JournalController : ControllerBase
{
    private readonly IJournalService _journalService;
    private readonly IResourceAccessHelper _resourceAccessHelper;

    public JournalController(IJournalService journalService, IResourceAccessHelper resourceAccessHelper)
    {
        _journalService = journalService;
        _resourceAccessHelper = resourceAccessHelper;
    }

    [HttpGet]
    [Route("entries")]
    public async Task<JsonResult> GetEntries([FromBody] PaginatedRequest request)
    {
        var results = await _journalService.GetEntriesAsync(request.ResultsPerPage,
            request.ResultsPerPage * (request.Page - 1));
        return new JsonResult(results);
    }

    [HttpPost]
    [Route("entry")]
    public async Task<JsonResult> CreateEntry([FromBody] JournalEntryRequest request)
    {
        await _resourceAccessHelper.ValidateActivityAccess(request.Activities);
        await _resourceAccessHelper.ValidateEmotionAccess(request.Emotions);
        var result =
            await _journalService.CreateEntry(request.Text, request.Emotions, request.Activities);
        return new JsonResult(new IdResponse(result));
    }

    [HttpGet]
    [Route("emotions")]
    public async Task<JsonResult> GetEmotions()
    {
        var results = await _journalService.GetEmotionsAsync();
        return new JsonResult(results);
    }

    [HttpPost]
    [Route("Emotions")]
    public async Task<JsonResult> CreateEmotion([FromBody] CreateEmotionRequest request)
    {
        var results = await _journalService.CreateEmotionAsync(request.Name, request.Category);
        return new JsonResult(new IdResponse(results));
    }

    [HttpGet]
    [Route("activities")]
    public async Task<JsonResult> GetActivities()
    {
        var results = await _journalService.GetActivitiesAsync();
        return new JsonResult(results);
    }

    [HttpPost]
    [Route("activities")]
    public async Task<JsonResult> CreateActivity([FromBody] NamedObjectRequest request)
    {
        var result = await _journalService.CreateActivityAsync(request.Name);
        return new JsonResult(new IdResponse(result));
    }

    [HttpDelete]
    [Route("activities")]
    public async Task<IActionResult> DeleteActivity([FromQuery] int id)
    {
        await _resourceAccessHelper.ValidateActivityAccess(id);
        await _journalService.DeleteActivityAsync(id);
        return StatusCode(204);
    }

    [HttpGet]
    [Route("emotioncategories")]
    public async Task<JsonResult> GetEmotionCategories()
    {
        var results = await _journalService.GetEmotionCategoriesAsync();
        return new JsonResult(results);
    }

    [HttpPost]
    [Route("emotioncategories")]
    public async Task<JsonResult> CreateEmotionCategory([FromBody] NamedObjectRequest request)
    {
        var result = await _journalService.CreateEmotionCategoryAsync(request.Name);
        return new JsonResult(new IdResponse(result));
    }
}