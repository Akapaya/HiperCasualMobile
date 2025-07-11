using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectPoolManager : MonoBehaviour
{
    [Header("References")]
    public static ObjectPoolManager Instance;
    [SerializeField] private string[] _addressableKeys = new string[10];

    [Header("Temp Data")]
    private Dictionary<string, object> _pools = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        foreach (var key in _addressableKeys)
        {
            if(key == string.Empty) { continue; }
            Addressables.LoadAssetAsync<GameObject>(key).Completed += handle =>
            {
                var prefab = handle.Result;
                CreatePool(key, prefab, 5);
            };
        }
    }

    public void CreatePool(string key, GameObject prefab, int initialSize = 5)
    {
        if (!_pools.ContainsKey(key))
        {
            Pool newPool = new(prefab, initialSize, transform);
            _pools[key] = newPool;
        }
    }

    public GameObject Get(string key)
    {
        if (_pools.TryGetValue(key, out var poolObj) && poolObj is Pool pool)
            return pool.Get();

        Debug.LogWarning($"Pool {key} Not Found");
        return default;
    }

    public void Return<T>(string key, GameObject obj)
    {
        if (_pools.TryGetValue(key, out var poolObj) && poolObj is Pool pool)
            pool.Return(obj);
    }
}
