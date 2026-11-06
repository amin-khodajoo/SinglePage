using System.ComponentModel.DataAnnotations;

namespace SinglePage.ApplicationServices.Dtos;

public class CreatePersonDto
{
    [Required, StringLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required, StringLength(100)] public string LastName { get; set; } = string.Empty;
}

public class UpdatePersonDto : CreatePersonDto
{
    [Range(1, int.MaxValue)] public int Id { get; set; }
}

public record PersonDto(int Id, string FirstName, string LastName);
