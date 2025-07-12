using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    public float MoveSpeed = 1.0f;
    public float RotationSpeed = 1.0f;
    public int Strenght = 1;
}
