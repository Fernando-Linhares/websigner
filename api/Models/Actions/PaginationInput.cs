namespace Api.Models.Actions;

public record PaginationInput<T>(int Page, int PerPage, int Total, IQueryable<T> Data);