using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
/// <summary>
/// Manages objects that will be reused during the game, objects are removed and returned to their pools through the Pool family.
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    [Header("References")]
    public static ObjectPoolManager Instance;
    [SerializeField] private string[] _addressableKeys = new string[10];

    [Header("Temp Data")]
    private Dictionary<string, object> _pools = new(10);
    private Dictionary<GameObject, IPoolUser> _activesObjects = new(10);

    #region Start Methods
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //Use addressables to populate pools, case in load runtime asset to pool.
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
    #endregion

    #region Pools methods
    /// <summary>
    /// Create Pool with Key to ID, prefab from addressables and initial size of pool.
    /// </summary>
    /// <param name="key">key to ID</param>
    /// <param name="prefab">Prefab to reference</param>
    /// <param name="initialSize">Initial size of pool</param>
    public void CreatePool(string key, GameObject prefab, int initialSize = 5)
    {
        if (!_pools.ContainsKey(key))
        {
            Pool newPool = new(prefab, initialSize, transform);
            _pools[key] = newPool;
        }
    }

    /// <summary>
    /// Get Object from Specific pool
    /// </summary>
    /// <param name="key">Key of family pool</param>
    /// <param name="poolUser">Script that request object</param>
    /// <returns>Object of pool</returns>
    public GameObject Get(string key, IPoolUser poolUser)
    {
        if (_pools.TryGetValue(key, out var poolObj) && poolObj is Pool pool)
        {
            GameObject obj = pool.Get();
            _activesObjects.Add(obj, poolUser);
            return obj;
        }

        Debug.LogWarning($"Pool {key} Not Found");
        return default;
    }

    /// <summary>
    /// Return object to pool
    /// </summary>
    /// <param name="key">Key of family pool</param>
    /// <param name="obj">object returning to pool</param>
    public void Return(string key, GameObject obj)
    {
        if (_pools.TryGetValue(key, out var poolObj) && poolObj is Pool pool)
        {
            obj.transform.SetParent(this.gameObject.transform);
            _activesObjects[obj].OnObjectsReturned(obj);
            _activesObjects.Remove(obj);
            pool.Return(obj);
        }
    }
    #endregion
}
