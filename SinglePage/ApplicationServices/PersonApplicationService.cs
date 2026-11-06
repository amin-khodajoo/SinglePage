using SinglePage.ApplicationServices.Contracts;
using SinglePage.ApplicationServices.Dtos;
using SinglePage.Models.DomainModels.PersonAggregate;
using SinglePage.Models.Services.Contracts;

namespace SinglePage.ApplicationServices;

public class PersonApplicationService(IPersonRepository repository) : IPersonApplicationService
{
    public async Task<List<PersonDto>> GetAll() => (await repository.SelectAll()).Select(ToDto).ToList();

    public async Task<PersonDto?> GetById(int id)
    {
        var person = await repository.SelectById(id);
        return person is null ? null : ToDto(person);
    }

    public async Task<PersonDto> Create(CreatePersonDto input)
    {
        var person = new Person { FirstName = input.FirstName.Trim(), LastName = input.LastName.Trim() };
        await repository.Insert(person);
        return ToDto(person);
    }

    public async Task<PersonDto?> Update(UpdatePersonDto input)
    {
        var person = await repository.SelectById(input.Id);
        if (person is null) return null;
        person.FirstName = input.FirstName.Trim();
        person.LastName = input.LastName.Trim();
        await repository.Edit(person);
        return ToDto(person);
    }

    public async Task<bool> Delete(int id)
    {
        var person = await repository.SelectById(id);
        if (person is null) return false;
        await repository.Delete(person);
        return true;
    }

    private static PersonDto ToDto(Person person) => new(person.Id, person.FirstName, person.LastName);
}
