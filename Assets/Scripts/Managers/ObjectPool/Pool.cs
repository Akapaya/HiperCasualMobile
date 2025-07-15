using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Pool settings with Prefab reference, and stack of pools.
/// </summary>
public class Pool
{
    private GameObject prefab;
    private Transform parent;
    private Stack<GameObject> poolStack = new();

    #region Pool Methods
    /// <summary>
    /// Pool Constructor
    /// </summary>
    /// <param name="parent">Object Pool Manager Parent</param>
    /// <param name="prefab">Prefab to reference</param>
    /// <param name="initialSize">Initial size of pool</param>
    public Pool(GameObject prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
            poolStack.Push(CreateNew());
    }

    /// <summary>
    /// Create new instance of object
    /// </summary>
    /// <returns>Object of pool</returns>
    private GameObject CreateNew()
    {
        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        return obj;
    }

    /// <summary>
    /// Give Object of pool to Manager
    /// </summary>
    /// <returns>Object of pool</returns>
    public GameObject Get()
    {
        GameObject obj = poolStack.Count > 0 ? poolStack.Pop() : CreateNew();
        obj.gameObject.SetActive(true);

        return obj;
    }

    /// <summary>
    /// Returning object to pool
    /// </summary>
    /// <param name="obj">object returning</param>
    public void Return(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        poolStack.Push(obj);
    }

    /// <summary>
    /// Clear all active objects of this family in scene.
    /// </summary>
    public void Clear()
    {
        foreach (var go in poolStack)
            GameObject.Destroy(go);

        poolStack.Clear();
    }
    #endregion
}
