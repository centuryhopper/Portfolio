
namespace Shared.Models;

public class BlogDTO
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string PreviewDesc { get; set; }
    public string RouteName { get; set; }
    public List<VideoUrlDTO>? VideoUrls { get; set; }
    public string FullDesc { get; set; }

    public override string ToString()
    {
        return $"{nameof(Title)}: {Title}, {nameof(Date)}: {Date}, {nameof(PreviewDesc)}: {PreviewDesc}, {nameof(RouteName)}: {RouteName},";
    }
}