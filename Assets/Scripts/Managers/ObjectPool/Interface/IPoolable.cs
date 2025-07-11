using UnityEngine;

public interface IPoolable
{
    public void OnPooled();
    public void OnDespawned();
    public void OnDestroy();
}
