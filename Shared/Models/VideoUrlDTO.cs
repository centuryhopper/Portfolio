namespace Shared.Models;



public class VideoUrlDTO
{
    public string url { get; set; }

    public override string ToString()
    {
        return $"url: {url}";
    }
}