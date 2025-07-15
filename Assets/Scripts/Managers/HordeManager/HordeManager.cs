using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Horde Manager to spawn enemies using Object Pool.
/// </summary>
public class HordeManager : MonoBehaviour, IUpdater, IPoolUser
{
    [Header("References")]
    [SerializeField] private HordeDataSO _hordeDataSO;
    [SerializeField] public static HordeManager Instance;

    [Header("Settings")]
    [SerializeField] private Transform[] _spawnPoints;

    [Header("Temp Data")]
    [SerializeField] private float _timer;
    [SerializeField] private List<GameObject> _activeEnemiesList = new(10);

    #region Start Methods
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdaterManager.Instance.AddIUpdaterInList(this);
    }
    #endregion

    #region IUpdater
    public void UpdateSection()
    {
        _timer += Time.deltaTime;

        if (_timer >= _hordeDataSO.SpawnInterval && _activeEnemiesList.Count < _hordeDataSO.MaxEnemies)
        {
            SpawnEnemy();
            _timer = 0f;
        }
    }
    #endregion

    #region Horde Methods
    /// <summary>
    /// Spawn Enemy getting of Object Pool in random position.
    /// </summary>
    void SpawnEnemy()
    {
        string randomID = _hordeDataSO.EnemiesID[Random.Range(0,_hordeDataSO.EnemiesID.Length)];
        if(randomID == string.Empty)
        {
            return;
        }

        GameObject enemyGO = ObjectPoolManager.Instance.Get(randomID, this);

        Transform point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        enemyGO.transform.position = point.position;
        enemyGO.transform.rotation = Quaternion.identity;

        _activeEnemiesList.Add(enemyGO);
    }
    #endregion

    #region IPoolUser
    public void OnObjectsReturned(GameObject obj)
    {
        _activeEnemiesList.Remove(obj);
    }
    #endregion
}