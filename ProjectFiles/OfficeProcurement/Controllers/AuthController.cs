using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Controllers;

/// <summary>
/// Контроллер аутентификации
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AuthController"/>
    /// </summary>
    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    /// <summary>
    /// Аутентифицирует пользователя и возвращает JWT-токен
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
    {
        var result = await authService.LoginAsync(request.Login, request.Password);
        if (result == null)
        {
            return Unauthorized(new { message = "Неверный логин или пароль" });
        }

        return Ok(result);
    }
}
