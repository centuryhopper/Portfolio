using Fluxor;

namespace Client.Store;

public static class BlogPageStateReducers
{
    [ReducerMethod]
    public static BlogPageState ReduceToggleIsNewestAction(BlogPageState state, ToggleIsNewestAction action)
    {
        return new BlogPageState(IsNewest: action.lastRecordedState ?? !state.IsNewest);
    }
}
