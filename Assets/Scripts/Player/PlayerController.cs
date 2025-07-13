using UnityEngine;

public class PlayerController : MonoBehaviour, IUpdater, IFixedUpdater
{
    [Header("References")]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private PlayerModel _playerModel;

    [Header("Temp Data")]
    [SerializeField] private Vector3 _moveInput;

    private void Start()
    {
        UpdaterManager.Instance.AddIUpdaterInList(this);
        UpdaterManager.Instance.AddIFixedUpdaterInList(this);
    }

    public void UpdateSection()
    {
        _moveInput = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
    }

    public void FixedUpdateSection()
    {
        _playerModel.MoveCharacter(_moveInput);
    }
}