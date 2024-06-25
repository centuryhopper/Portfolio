namespace Portfolio.Entities;


public class VideoUrlModel
{
    public string url { get; set; }
    public string title { get; set; }

    public override string ToString()
    {
        return $"title: {title}, url: {url}";
    }
}