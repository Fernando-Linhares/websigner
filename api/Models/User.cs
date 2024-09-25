using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class User
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Collection<Token> Tokens { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}