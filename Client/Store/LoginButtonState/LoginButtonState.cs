
using Fluxor;

namespace Client.Store;

// this attr is not needed if BlogPageFeature class exists
[FeatureState]
public class LoginButtonState
{
    public bool IsLoginButtonVisible { get; }

    private LoginButtonState() { } // Required for creating initial state

    // Constructor to initialize the state
    public LoginButtonState(bool IsLoginButtonVisible)
    {
        this.IsLoginButtonVisible = IsLoginButtonVisible;
    }

}
