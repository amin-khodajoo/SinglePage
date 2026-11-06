using SinglePage.ApplicationServices.Dtos;

namespace SinglePage.ApplicationServices.Contracts;

public interface IPersonApplicationService
{
    Task<List<PersonDto>> GetAll();
    Task<PersonDto?> GetById(int id);
    Task<PersonDto> Create(CreatePersonDto input);
    Task<PersonDto?> Update(UpdatePersonDto input);
    Task<bool> Delete(int id);
}
