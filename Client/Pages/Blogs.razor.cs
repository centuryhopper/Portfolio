
using System.Net.Http.Json;
using Client.Store;
using Client.Utils;
using Fluxor;
using Client.Layout;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using static Shared.Models.ServiceResponses;
using HandyBlazorComponents.Components;
using HandyBlazorComponents.Models;

namespace Client.Pages;

public partial class Blogs
{
    [Inject] IState<BlogPageState> BlogPageState {get;set;}
    [Inject] IDispatcher Dispatcher {get;set;}
    protected NotificationModal notificationModal = default!;
    protected ConfirmModal confirmModal = default!;
    protected IEnumerable<BlogDTO>? BlogCards;
    protected bool IsNewest = false;

    protected HttpClient httpClient;

    protected override async Task OnInitializedAsync()
    {
        // Load sorting preference from session storage
        var lastRecordedBlogSortState = await sessionStorageService.GetItemAsync<bool?>(nameof(BlogPageState.Value.IsNewest));

        var action = new ToggleIsNewestAction(lastRecordedState: lastRecordedBlogSortState ?? false);
        Dispatcher.Dispatch(action);

        // Save the new sorting preference
        await sessionStorageService.SetItemAsync(nameof(BlogPageState.Value.IsNewest), BlogPageState.Value.IsNewest);

        // Initialize the HttpClient
        httpClient = httpClientFactory.CreateClient(Constants.HTTP_CLIENT);

        // Fetch blogs from API
        await LoadBlogs();
    }

    private async Task LoadBlogs()
    {
        BlogCards = await httpClient.GetFromJsonAsync<IEnumerable<BlogDTO>>("api/Blogs/get-blogs");
        // Apply the sorting based on the saved state
        ApplySorting();
    }

    private void ApplySorting()
    {
        if (BlogPageState.Value.IsNewest)
        {
            BlogCards = BlogCards?.OrderByDescending(x => x.Date);
        }
        else
        {
            BlogCards = BlogCards?.OrderBy(x => x.Date);
        }
        // need this to make it work
        StateHasChanged();
    }

    private async Task DeleteBlog(int blogId)
    {
        var isConfirmed = await confirmModal.ShowAsync();
        if (!isConfirmed)
        {
            ApplySorting();
            //Console.WriteLine("Cancelled delete operation");
            return;
        }

        var response = await httpClient.DeleteAsync($"api/Blogs/delete-blogs/{blogId}");
        var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();

        if (!result.Flag)
        {
            await notificationModal.ShowAsync("Error", result.Message, HandyModalType.ERROR);
        }

        await notificationModal.ShowAsync("Success", result.Message, HandyModalType.SUCCESS);
        await LoadBlogs();  // Reload the blogs after deletion
    }

    private async void HandleSort()
    {
        // Toggle sorting order
        // IsNewest = !IsNewest;


        var action = new ToggleIsNewestAction();

        // Dispatch an action to toggle the IsNewest state
        Dispatcher.Dispatch(action);
        
        // Save the new sorting preference
        await sessionStorageService.SetItemAsync(nameof(BlogPageState.Value.IsNewest), BlogPageState.Value.IsNewest);
        
        Console.WriteLine(BlogPageState.Value.IsNewest);
        // Apply sorting
        ApplySorting();
    }
}
