using UnityEngine;

public class CameraManager : MonoBehaviour, ILateUpdater
{
    [Header("References")]
    [SerializeField] private Transform _target;
    [SerializeField] private CameraDataSO _cameraDataSO;
    [SerializeField] private Camera _camera;

    private void Start()
    {
        UpdaterManager.Instance.AddILateUpdaterInList(this);
    }

    public void LateUpdateSection()
    {
        if (!_target) return;

        Vector3 targetPosition = _target.position + _cameraDataSO.Offset;
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, _cameraDataSO.FollowSpeed * Time.deltaTime);
    }
}
