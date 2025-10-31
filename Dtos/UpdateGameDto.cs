using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record class UpdateGameDto(
        [Required][StringLength(50)] string Name,
        int GenreId,
        [Required][Range(1, 500)] decimal Price,
        DateOnly ReleaseDate
    );
