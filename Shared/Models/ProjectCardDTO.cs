using System.ComponentModel.DataAnnotations;

namespace Shared.Models;



public class ProjectCardDTO
{
    public int Id { get; set; }
    [Required]
    public string? ImgUrl { get; set; }
    [Required]
    public string? Title { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public string? ProjectLink { get; set; }
    [Required]
    public string? SourceCodeLink { get; set; }

    public override string ToString()
    {
        return $"{nameof(ImgUrl)}:{ImgUrl}, {nameof(Title)}:{Title}, {nameof(Description)}:{Description}, {nameof(ProjectLink)}:{ProjectLink}, {nameof(SourceCodeLink)}:{SourceCodeLink}, ";
    }

}