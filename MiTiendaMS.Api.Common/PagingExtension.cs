using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MiTiendaMS.Api.Common
{
    public static class PagingExtension
    {
        public static async Task<PagedCollection<T>> GetPagedAsync<T>(
            this IQueryable<T> query,
            int page,
            int take)
        {
            var originalPages = page;
            page--;

            if (page > 0)
            {
                page *= take;
            }

            var result = new PagedCollection<T>
            {
                Items = await query.Skip(page).Take(take).ToListAsync(),
                Total = await query.CountAsync(),
                Page = originalPages
            };

            if (result.Total > 0)
            {
                result.Pages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(result.Total) / take));
            }
            return result;
        }
    }
}
