using UnityEngine;

public class PlayerController : MonoBehaviour, IUpdater
{
    [Header("References")]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private PlayerModel _playerModel;

    private void Start()
    {
        UpdaterManager.Instance.AddIUpdaterInList(this);
    }

    public void UpdateSection()
    {
        Vector3 move = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        _playerModel.MoveCharacter(move);
    }
}