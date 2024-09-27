namespace Api.Models.Actions;

public record PaginationOutput<T>(List<T> Data, Object Pagination);