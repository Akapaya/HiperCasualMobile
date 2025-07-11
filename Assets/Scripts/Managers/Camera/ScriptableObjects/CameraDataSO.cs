using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "CameraData/CameraDataSO")]
public class CameraDataSO : ScriptableObject
{
    public Vector3 Offset = new Vector3(0, 0, 0);
    public float FollowSpeed = 1f;
}
