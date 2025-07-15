using UnityEngine;
/// <summary>
/// Data for enemy id, stats and inventory
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    public float MoveSpeed = 1.0f;
    public float RotationSpeed = 1.0f;
    public int MaxStack = 1;
    public int Strenght = 1;
    public int Coins = 0;
}
