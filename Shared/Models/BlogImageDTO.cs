using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class BlogImageDTO
{
    public int? ImageId { get; set; }
    public int? BlogId { get; set; }
    [Required]
    public byte[] ImageData { get; set; }
    [Required]
    public string ContentType { get; set; }
    public DateTime UploadDate {get;set;} = DateTime.Now;
}