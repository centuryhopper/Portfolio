
using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class BlogTinymceDTO
{
    public int? Id { get; set; }
    [Required]
    public string Title { get; set; }
    public DateTime? Date { get; set; }
    public string? PreviewDesc { get; set; }
    public string? RouteName { get; set; }
    [Required]
    public string FullDesc { get; set; }

    public override string ToString()
    {
        return $"{nameof(Title)}: {Title}, {nameof(Date)}: {Date}, {nameof(PreviewDesc)}: {PreviewDesc}, {nameof(RouteName)}: {RouteName},";
    }
}