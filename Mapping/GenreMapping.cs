using WebApplication1.Dtos;
using WebApplication1.Entities;

namespace WebApplication1.Mapping
{
    public static class GenreMapping
    {
        public static GenreDto ToDto(this Genre genre)
        {
            return new (
                genre.Id,
                genre.Name
            );

        }
    }
}
