namespace Shared.Models;



public class SkillDTO
{
    public string Title { get; set; }
    public IEnumerable<string> Descriptions { get; set; }

    public override string ToString()
    {
        return $"title: {Title}. Descriptions may be long so they won't be printed";
    }
}