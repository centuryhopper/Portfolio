
using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class BlogDTO
{
    public int? Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public DateOnly Date { get; set; }
    [Required]
    public string FullDesc { get; set; }
    [Required]
    public string PreviewDesc { get; set; }

    public void ResetValues()
    {
        Title = default!;
        Date = default!;
        FullDesc = default!;
        PreviewDesc = default!;
    }

    public override string ToString()
    {
        return $"{nameof(Title)}: {Title}, {nameof(Date)}: {Date}, {nameof(PreviewDesc)}: {PreviewDesc}";
    }
}