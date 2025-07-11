using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData/EnemyDataSO", order = 1)]
public class EnemyDataSO : ScriptableObject
{
    public float MoveSpeed = 3f;
    public int MaxHealth = 3;
    public Vector2 PatrolAreaMin = new Vector2(-10, -10);
    public Vector2 PatrolAreaMax = new Vector2(10, 10);
    public float PointReachDistance = 0.5f;
    public float StopTime = 0f;
}
