using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using ModelFile=Api.Models.File;

namespace Api.Models;

public class SignatureProcess
{
    [Key]
    public long Id { get; set; }
    public long UserId { get; set; }
    public Enums.SignatureStatus Status { get; set; } = Enums.SignatureStatus.Pending;
    public DateTime Date { get; set; }
    public Collection<ModelFile> Files { get; set; }
}