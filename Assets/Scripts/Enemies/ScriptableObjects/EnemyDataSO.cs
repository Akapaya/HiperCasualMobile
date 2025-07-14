using UnityEngine;

/// <summary>
/// Data for enemy id, stats and behaviours
/// </summary>
[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData/EnemyDataSO", order = 1)]
public class EnemyDataSO : ScriptableObject
{
    public string EnemyID;
    public float MoveSpeed = 3f;
    public int MaxHealth = 3;
    public int AmountDropCoins = 3;
    public float PointReachDistance = 0.5f;
    public float StopTime = 0f;
}
