
using Client.Store;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Client.Providers;

namespace Client.Layout;

public partial class MainLayout : IDisposable
{
    protected int cnt = 0;
    protected bool isAuthenticated;

    [Inject] protected IState<LoginButtonState> LoginButtonIState {get;set;}
    [Inject] protected IDispatcher Dispatcher {get;set;}

    private void IncrementCount()
    {
        if (cnt > 10)
        {
            return;
        }
        cnt++;
        Console.WriteLine(cnt);
        if (cnt > 10)
        {
            Dispatcher.Dispatch(new ToggleLoginButtonUnlockedAction(true));
            //Console.WriteLine(LoginButtonIState.Value.IsLoginButtonVisible.ToString());
            sessionStorageService.SetItemAsync(nameof(LoginButtonIState.Value.IsLoginButtonVisible), LoginButtonIState.Value.IsLoginButtonVisible);
        }

    }
    protected override async Task OnInitializedAsync()
    {
        //Console.WriteLine(LoginButtonIState.Value.IsLoginButtonVisible.ToString());
        var authState = await ((ApiAuthenticationStateProvider) AuthenticationStateProvider).GetAuthenticationStateAsync();
        isAuthenticated = authState.User.Identity?.IsAuthenticated ?? false;
        var previousLoginButtonStateIsVisible = await sessionStorageService.GetItemAsync<bool>(nameof(LoginButtonIState.Value.IsLoginButtonVisible));
        var action = new ToggleLoginButtonUnlockedAction(lastRecordedState: previousLoginButtonStateIsVisible);
        Dispatcher.Dispatch(action);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            LoginButtonIState.StateChanged += StateChanged;
        }
    }

    public void StateChanged(object sender, EventArgs args)
    {
        InvokeAsync(StateHasChanged);
    }

    void IDisposable.Dispose()
    {
        LoginButtonIState.StateChanged -= StateChanged;
    }
}