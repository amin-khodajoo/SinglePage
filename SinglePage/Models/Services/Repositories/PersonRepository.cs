using Microsoft.EntityFrameworkCore;
using SinglePage.Models.DomainModels.PersonAggregate;
using SinglePage.Models.Services.Contracts;

namespace SinglePage.Models.Services.Repositories;

public class PersonRepository(ProjectDbContext db) : IPersonRepository
{
    public Task<List<Person>> SelectAll() => db.Person.AsNoTracking().OrderBy(p => p.Id).ToListAsync();
    public async Task<Person?> SelectById(int id) => await db.Person.FindAsync(id);

    public async Task Insert(Person person)
    {
        db.Person.Add(person);
        await db.SaveChangesAsync();
    }

    public async Task Edit(Person person)
    {
        db.Person.Update(person);
        await db.SaveChangesAsync();
    }

    public async Task Delete(Person person)
    {
        db.Person.Remove(person);
        await db.SaveChangesAsync();
    }
}
