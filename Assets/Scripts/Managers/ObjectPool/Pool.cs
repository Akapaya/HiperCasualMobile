using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private GameObject prefab;
    private Transform parent;
    private Stack<GameObject> poolStack = new();

    public Pool(GameObject prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
            poolStack.Push(CreateNew());
    }

    private GameObject CreateNew()
    {
        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        return obj;
    }

    public GameObject Get()
    {
        GameObject obj = poolStack.Count > 0 ? poolStack.Pop() : CreateNew();
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        poolStack.Push(obj);
    }

    public void Clear()
    {
        foreach (var go in poolStack)
            GameObject.Destroy(go);

        poolStack.Clear();
    }
}
