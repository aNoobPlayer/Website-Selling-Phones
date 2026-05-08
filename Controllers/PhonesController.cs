using Microsoft.AspNetCore.Mvc;
using Website_Selling_Phones.Services;

namespace Website_Selling_Phones.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhonesController : ControllerBase
{
    private readonly MockDataService _mockData;

    public PhonesController(MockDataService mockData)
    {
        _mockData = mockData;
    }

    [HttpGet]
    public IActionResult GetPhones([FromQuery] string? brand, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice,
        [FromQuery] string? category, [FromQuery] string? condition)
    {
        var phones = _mockData.FilterPhones(brand, minPrice, maxPrice, category, condition);
        return Ok(phones);
    }

    [HttpGet("{id}")]
    public IActionResult GetPhone(int id)
    {
        var phone = _mockData.GetPhoneById(id);
        if (phone == null)
            return NotFound();
        return Ok(phone);
    }

    [HttpGet("brands")]
    public IActionResult GetBrands()
    {
        return Ok(_mockData.GetBrands());
    }

    [HttpGet("categories")]
    public IActionResult GetCategories()
    {
        return Ok(_mockData.GetCategories());
    }
}