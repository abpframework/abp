namespace Volo.Abp.IdentityServer.AspNetIdentity;

public class IdentityUserStoreFailureSimulator
{
    private int? _successfulUpdatesBeforeFailure;

    public bool IsAccessFailedCountResetFailureEnabled { get; private set; }

    public void FailAccessFailedCountReset()
    {
        IsAccessFailedCountResetFailureEnabled = true;
    }

    public void FailAfterSuccessfulUpdates(int successfulUpdateCount)
    {
        _successfulUpdatesBeforeFailure = successfulUpdateCount;
    }

    public bool ShouldFailUpdate()
    {
        if (!_successfulUpdatesBeforeFailure.HasValue)
        {
            return false;
        }

        if (_successfulUpdatesBeforeFailure.Value > 0)
        {
            _successfulUpdatesBeforeFailure--;
            return false;
        }

        _successfulUpdatesBeforeFailure = null;
        return true;
    }

    public void Reset()
    {
        IsAccessFailedCountResetFailureEnabled = false;
        _successfulUpdatesBeforeFailure = null;
    }
}
