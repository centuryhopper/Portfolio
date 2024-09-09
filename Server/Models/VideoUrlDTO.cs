namespace Shared.Models;



public class VideoUrlDTO
{
    public string url { get; set; }
    public string title { get; set; }

    public override string ToString()
    {
        return $"title: {title}, url: {url}";
    }
}