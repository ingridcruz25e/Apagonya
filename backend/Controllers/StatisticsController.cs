using ApagonYa.Api.Services; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc;
namespace ApagonYa.Api.Controllers;
[ApiController][Route("api/[controller]")] public class StatisticsController : ControllerBase { private readonly StatisticsService _service; public StatisticsController(StatisticsService service)=>_service=service; [Authorize(Roles="administrador")][HttpGet] public async Task<IActionResult> Get()=>Ok(await _service.GetStatisticsAsync()); }
