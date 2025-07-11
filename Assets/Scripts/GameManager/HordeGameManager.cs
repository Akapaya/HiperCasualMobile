using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HordeGameManager : MonoBehaviour, IUpdater
{
    [Header("References")]
    [SerializeField] private HordeDataSO _hordeDataSO;

    [Header("Settings")]
    [SerializeField] private Transform[] _spawnPoints;

    [Header("Temp Data")]
    private float timer;
    private Dictionary<GameObject,string> activeEnemies = new(10);

    #region Start Methods
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

        GameObject enemyGO = ObjectPoolManager.Instance.Get(randomID);

        Transform point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        enemyGO.transform.position = point.position;
        enemyGO.transform.rotation = Quaternion.identity;

        activeEnemies[enemyGO] = randomID;
    }

    public void DespawnEnemy(GameObject enemyGO)
    {
        ObjectPoolManager.Instance.Return(activeEnemies[enemyGO], enemyGO);
        activeEnemies.Remove(enemyGO);
    }

    public void DespawnAllEnemies()
    {
        for(int i = 0; i< activeEnemies.Count; i++)
        {
            DespawnEnemy(activeEnemies.ElementAt(i).Key);
        }

        activeEnemies.Clear();
    }
    #endregion
}