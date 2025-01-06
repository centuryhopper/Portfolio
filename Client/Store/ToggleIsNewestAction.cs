
namespace Client.Store;

public class ToggleIsNewestAction
{
    public bool? lastRecordedState { get; set; } = null;

    public ToggleIsNewestAction(bool? lastRecordedState = null)
    {
        this.lastRecordedState = lastRecordedState;
    }

}
