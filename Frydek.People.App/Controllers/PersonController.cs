using Frydek.People.Application.Dtos;
using Frydek.People.Application.UseCases;
using Frydek.People.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Frydek.People.App.Controllers;

[ApiController]
[Route("api/person")]
public class PersonController(
    IGetPersonUseCase getPersonUseCase,
    ICreatePersonUseCase createPersonUseCase,
    IGetAllPersonsUseCase getAllPersonsUseCase,
    IUpdatePersonUseCase updatePersonUseCase,
    IDeletePersonUseCase deletePersonUseCase
) : Controller
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var person = await getPersonUseCase.ExecuteAsync(Guid.Parse(id));

            return Ok(person);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var people = await getAllPersonsUseCase.ExecuteAsync();

        return Ok(people);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonDto dto)
    {
        var person = await createPersonUseCase.ExecuteAsync(dto);

        return CreatedAtAction(nameof(Get), new { id = person.Id }, person);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdatePersonDto dto)
    {
        try
        {
            var person = await updatePersonUseCase.ExecuteAsync(Guid.Parse(id), dto);

            return Ok(person);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await deletePersonUseCase.ExecuteAsync(Guid.Parse(id));

            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

}