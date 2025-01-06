
namespace Client.Store;

public class ToggleLoginButtonUnlockedAction
{
    public bool? lastRecordedState { get; set; } = null;

    public ToggleLoginButtonUnlockedAction(bool? lastRecordedState = null)
    {
        this.lastRecordedState = lastRecordedState;
    }
}
