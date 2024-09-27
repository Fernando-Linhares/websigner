using System.ComponentModel.DataAnnotations;
using Api.Models.Actions;

namespace Api.Models;

public class Certificate
{
    [Key]
    public long Id { get; set; }
    public string FileName { get; set; }
    public string Alias { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public async Task<PaginationOutput<Certificate>> Paginate(PaginationInput<Certificate> input)
    {
        var paginateAsync = new PaginateAsync<Certificate>();
        return await paginateAsync.Execute(input);
    }
}