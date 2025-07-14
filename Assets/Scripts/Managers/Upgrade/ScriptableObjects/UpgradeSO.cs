using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade/UpgradeSO", order = 1)]
public class UpgradeSO : ScriptableObject
{
    public int Cost;
    public string Description;
    public int Stack;
    public int Strenght;
    public int Speed;
}