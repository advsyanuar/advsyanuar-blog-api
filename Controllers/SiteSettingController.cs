using Microsoft.AspNetCore.Mvc;
using portfolio_api.Helpers;
using portfolio_api.Models;

namespace portfolio_api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SiteSettingController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var jsonFileReader = new JsonFileReader<SiteSettings>("site_setting.json");
            var result = await jsonFileReader.ReadAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
