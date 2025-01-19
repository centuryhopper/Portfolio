
namespace Client.Utils;
using Ganss.Xss;
using Microsoft.AspNetCore.Components;

public static class Helpers
{
    public static MarkupString SanitizeHtmlContent(string htmlContent)
    {
        string SanitizedBlogContent = string.Empty;
        try
        {
            // Create an instance of HtmlSanitizer
            var sanitizer = new HtmlSanitizer();

            // Optional: Configure sanitizer for additional rules
            sanitizer.AllowedTags.Add("h1");
            sanitizer.AllowedTags.Add("p");
            sanitizer.AllowedTags.Add("b");

            // Sanitize the raw HTML
            SanitizedBlogContent = sanitizer.Sanitize(htmlContent);
        }
        catch (Exception _)
        {
            SanitizedBlogContent = "Error sanitizing HTML.";
        }

        return new(SanitizedBlogContent);
    }
}
