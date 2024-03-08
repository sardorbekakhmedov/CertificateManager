using Certificate.Application.Abstractions.Interfaces;
using Certificate.Application.SortFilters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Certificate.Application.Extensions;

public static class QueryableExtensions
{
    public static async Task<IEnumerable<TEntity>> ToPagedListAsync<TEntity>(this IQueryable<TEntity> source,
        IHttpContextHelper httpContextHelper, PaginationParams filterParams)
    {
        var content = JsonConvert.SerializeObject(
            new PaginationMetaData(source.Count(), filterParams.PageSize, filterParams.Page));

        httpContextHelper.AddResponseToHeaderData("X-Pagination", content);

        return await source.Skip(filterParams.PageSize * (filterParams.Page - 1)).Take(filterParams.PageSize).ToListAsync();
    }
}