
using Fluxor;

namespace Client.Store;

// this attr is not needed if BlogPageFeature class exists
[FeatureState]
public class BlogPageState
{
    public bool IsNewest { get; } = true; // see the most recent one by default

    private BlogPageState() {} // Required for creating initial state

    // Constructor to initialize the state
    public BlogPageState(bool IsNewest)
    {
        this.IsNewest = IsNewest;
    }

}
