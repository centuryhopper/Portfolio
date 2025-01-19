using System.ComponentModel.DataAnnotations;

namespace Shared.Models;



public class VideoUrlDTO
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    [Required]
    public string Url { get; set; }

    public override string ToString()
    {
        return $"url: {Url}";
    }
}