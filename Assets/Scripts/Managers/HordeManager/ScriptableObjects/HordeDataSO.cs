using UnityEngine;

[CreateAssetMenu(fileName = "HordeData", menuName = "HordeData/HordeDataSO", order = 1)]
public class HordeDataSO : ScriptableObject
{
    public float SpawnInterval = 2f;
    public int MaxEnemies = 10;
    public string[] EnemiesID = new string[10];
}