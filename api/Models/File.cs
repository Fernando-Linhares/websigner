using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class File
{
    [Key]
    public long Id { get; set; }
    public string FileName { get; set; }
    public SignatureProcess SignatureProcess { get; set; }
    public long SignatureProcessId { get; set; }
}