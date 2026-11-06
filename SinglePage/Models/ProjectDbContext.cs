using Microsoft.EntityFrameworkCore;
using SinglePage.Models.DomainModels.PersonAggregate;

namespace SinglePage.Models;

public class ProjectDbContext(DbContextOptions<ProjectDbContext> options) : DbContext(options)
{
    public DbSet<Person> Person => Set<Person>();
}
