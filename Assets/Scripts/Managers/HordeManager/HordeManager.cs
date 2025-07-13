using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HordeManager : MonoBehaviour, IUpdater, IPoolUser
{
    [Header("References")]
    [SerializeField] private HordeDataSO _hordeDataSO;
    [SerializeField] public static HordeManager Instance;

    [Header("Settings")]
    [SerializeField] private Transform[] _spawnPoints;

    [Header("Temp Data")]
    private float timer;
    private List<GameObject> activeEnemies = new(10);

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
        timer += Time.deltaTime;

        if (timer >= _hordeDataSO.SpawnInterval && activeEnemies.Count < _hordeDataSO.MaxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }
    #endregion

    #region Horde Methods
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

        activeEnemies.Add(enemyGO);
    }
    #endregion

    #region IPoolUser
    public void OnObjectsReturned(GameObject obj)
    {
        activeEnemies.Remove(obj);
    }
    #endregion
}