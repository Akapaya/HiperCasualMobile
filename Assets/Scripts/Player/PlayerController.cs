using UnityEngine;
/// <summary>
/// Handle with player controllers input using IUpdater and IFixed Updater
/// </summary>
public class PlayerController : MonoBehaviour, IUpdater, IFixedUpdater
{
    [Header("References")]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private PlayerModel _playerModel;

    [Header("Temp Data")]
    [SerializeField] private Vector3 _moveInput;

    #region Start Methods
    private void Start()
    {
        UpdaterManager.Instance.AddIUpdaterInList(this);
        UpdaterManager.Instance.AddIFixedUpdaterInList(this);
    }
    #endregion

    #region Updaters Methods
    public void UpdateSection()
    {
        _moveInput = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
    }

    public void FixedUpdateSection()
    {
        _playerModel.MoveCharacter(_moveInput);
    }
    #endregion
}