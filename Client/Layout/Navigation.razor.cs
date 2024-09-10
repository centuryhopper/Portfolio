using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
 
namespace Client.Layout
{
    public partial class Navigation
    {
        [Parameter] public RenderFragment ChildContent { get; set; } = default!;
    }
}