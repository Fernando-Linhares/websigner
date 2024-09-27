using System.ComponentModel.DataAnnotations;
using Api.Models.Actions;

namespace Api.Models;

public class File
{
    [Key]
    public long Id { get; set; }
    public string FileName { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public string Url()
    {
        var config = new ConfigApp();
        return config.Get("app.url") + "pdfs/" + FileName;
    }

    public long CreatedAtTimestamp()
    {
        DateTimeOffset dateTimeOffset = new DateTimeOffset(CreatedAt);
        return dateTimeOffset.ToUnixTimeSeconds();
    }

    public async Task<PaginationOutput<File>> Paginate(PaginationInput<File> input)
    {
        var paginator = new PaginateAsync<File>();
        return await paginator.Execute(input);
    }
}