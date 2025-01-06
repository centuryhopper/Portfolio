using Fluxor;

namespace Client.Store;

public static class LoginButtonReducers
{
    [ReducerMethod]
    public static LoginButtonState ReduceToggleLoginButtonUnlocked(LoginButtonState _, ToggleLoginButtonUnlockedAction action)
    {
        // once it's true, keep it that way for the lifetime of the app session
        return new LoginButtonState(IsLoginButtonVisible: action.lastRecordedState ?? true);
    }
}
