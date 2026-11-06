using SinglePage.Models.DomainModels.PersonAggregate;

namespace SinglePage.Models.Services.Contracts;

public interface IPersonRepository
{
    Task<List<Person>> SelectAll();
    Task<Person?> SelectById(int id);
    Task Insert(Person person);
    Task Edit(Person person);
    Task Delete(Person person);
}
