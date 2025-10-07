using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallengeGames.Application.Games.Commands;
using TechChallengeGames.Application.Games.Interfaces;
using TechChallengeGames.Domain.Dto;

namespace TechChallengeGames.Api.Controllers;

[Route("api/[controller]")]
public class GamesController(IGameService service, ILogger<GamesController> logger) : BaseController(logger)
{
    [HttpGet]
    [Authorize("User")]
    [Route("{id:Guid}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get([FromRoute] Guid id)
        => Send(service.Find(id));
    
    [HttpGet]
    [Authorize("User")]
    [Route("[action]/{id:Guid}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Recommendation([FromRoute] Guid id)
        => Send(await service.Recommendations(id));
    
    [HttpGet]
    [Authorize("User")]
    [Route("[action]")]
    [ProducesResponseType(typeof(TopGameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Top10()
        => Send(await service.Top10());
    
    [HttpGet]
    [Authorize("User")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Get()
        => Send(service.Find());
    
    [HttpPost]
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CreateGameRequest request)
        => await Send(service.CreateAsync(request));
    
    [HttpPut]
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put([FromBody] UpdateGameRequest request)
        => await Send(service.UpdateAsync(request));
    
    [HttpDelete]
    [Authorize("Admin")]
    [Route("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
        => await Send(service.DeleteAsync(id));
}