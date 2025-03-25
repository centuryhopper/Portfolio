
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
    private HandyToast handyToast = default!;
    protected List<BlogDTO>? BlogCards;
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
        BlogCards = (await httpClient.GetFromJsonAsync<IEnumerable<BlogDTO>>("api/Blogs/get-blogs")).ToList();
        // Apply the sorting based on the saved state
        ApplySorting();
    }

    private void ApplySorting()
    {
        if (BlogPageState.Value.IsNewest)
        {
            BlogCards = BlogCards?.OrderByDescending(x => x.Date).ToList();
        }
        else
        {
            BlogCards = BlogCards?.OrderBy(x => x.Date).ToList();
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

        var response = await httpClient.DeleteFromJsonAsync<GeneralResponse>($"api/Blogs/delete-blog/{blogId}");
        if (!response.Flag)
        {
            _ = handyToast.ShowToastAsync("Error", response.Message, HandyToastType.ERROR);
        }
        else
        {
            _ = handyToast.ShowToastAsync("Success", response.Message, HandyToastType.SUCCESS);
            var toDelete = BlogCards.Find(x=>x.Id == blogId);
            if (toDelete is not null)
            {
                BlogCards.Remove(toDelete);
                StateHasChanged();
            }
        }
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
