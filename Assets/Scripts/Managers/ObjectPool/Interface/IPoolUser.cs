using UnityEngine;
/// <summary>
/// Interface for Pool User handled when object return to pool
/// </summary>
public interface IPoolUser
{
    public void OnObjectsReturned(GameObject obj);
}
