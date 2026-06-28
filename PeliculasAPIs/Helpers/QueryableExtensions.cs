using PeliculasAPIs.DTOs;

namespace PeliculasAPIs.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDto paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.CantidadRegistrosPorPagina)
                .Take(paginacionDTO.CantidadRegistrosPorPagina);
        }
    }
}
