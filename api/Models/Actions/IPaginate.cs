namespace Api.Models.Actions;

public interface IPaginate<T>
{
    public Task<PaginationOutput<T>> Execute(PaginationInput<T> input);
}