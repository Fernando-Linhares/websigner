using Microsoft.EntityFrameworkCore;

namespace Api.Models.Actions;

public class PaginateAsync<T>: IPaginate<T>
{
    public async Task<PaginationOutput<T>> Execute(PaginationInput<T> input)
    {
        var data = await input.Data
            .Skip((input.Page - 1) * input.PerPage)
            .Take(input.PerPage)
            .ToListAsync(); 
        
        int pages = (int) Math.Ceiling((decimal) input.Total /  input.PerPage);

        return new PaginationOutput<T>(data, new
        {
            page = input.Page,
            next = input.Page + (input.Page != pages ? 1 : 0),
            count = data.Count,
            pages = pages,
            total = input.Total
        });
    }
}