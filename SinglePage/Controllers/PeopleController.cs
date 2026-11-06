using Microsoft.AspNetCore.Mvc;
using SinglePage.ApplicationServices.Contracts;
using SinglePage.ApplicationServices.Dtos;

namespace SinglePage.Controllers;

[ApiController]
[Route("api/people")]
public class PeopleController(IPersonApplicationService service) : ControllerBase
{
    [HttpGet]
    public Task<List<PersonDto>> GetAll() => service.GetAll();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonDto>> GetById(int id)
    {
        var person = await service.GetById(id);
        return person is null ? NotFound() : Ok(person);
    }

    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create(CreatePersonDto input)
    {
        var person = await service.Create(input);
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PersonDto>> Update(int id, UpdatePersonDto input)
    {
        if (id != input.Id) return BadRequest(new { message = "شناسه مسیر و اطلاعات ارسالی یکسان نیستند." });
        var person = await service.Update(input);
        return person is null ? NotFound() : Ok(person);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => await service.Delete(id) ? NoContent() : NotFound();
}
