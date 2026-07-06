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
    IDeletePersonUseCase deletePersonUseCase
) : Controller
{
    private IGetPersonUseCase GetPersonUseCase { get; } = getPersonUseCase;
    private ICreatePersonUseCase CreatePersonUseCase { get; } = createPersonUseCase;
    private IGetAllPersonsUseCase GetAllPersonsUseCase { get; } = getAllPersonsUseCase;
    private IDeletePersonUseCase DeletePersonUseCase { get; } = deletePersonUseCase;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var person = await GetPersonUseCase.ExecuteAsync(Guid.Parse(id));

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
        var people = await GetAllPersonsUseCase.ExecuteAsync();

        return Ok(people);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonDto dto)
    {
        var person = await CreatePersonUseCase.ExecuteAsync(dto);

        return CreatedAtAction(nameof(Get), new { id = person.Id }, person);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await DeletePersonUseCase.ExecuteAsync(Guid.Parse(id));

            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

}