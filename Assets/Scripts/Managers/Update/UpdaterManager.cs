using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manager to centralize updates for the entire game, to avoid multiple C++ method calls and optimize the game.
/// </summary>
public class UpdaterManager : MonoBehaviour
{
    [Header("References")]
    public static UpdaterManager Instance;

    [Header("TempData")]
    private List<IUpdater> _updaters = new(10);
    private List<IFixedUpdater> _fixedUpdaters = new(10);
    private List<ILateUpdater> _lateUpdaters = new(10);

    #region Start Methods
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    #region Registers Methods
    public void AddIUpdaterInList(IUpdater updater)
    {
        _updaters.Add(updater);
    }

    public void RemoveIUpdaterInList(IUpdater updater)
    {
        _updaters.Remove(updater);
    }

    public void AddIFixedUpdaterInList(IFixedUpdater updater)
    {
        _fixedUpdaters.Add(updater);
    }

    public void AddILateUpdaterInList(ILateUpdater updater)
    {
        _lateUpdaters.Add(updater);
    }
    #endregion

    #region Updaters Methods
    void Update()
    {
        for (int i = _updaters.Count - 1; i >= 0; i--)
        {
            if (_updaters[i] != null)
            {
                _updaters[i].UpdateSection();
            }
        }
    }

    void LateUpdate()
    {
        for (int i = _lateUpdaters.Count - 1; i >= 0; i--)
        {
            if (_lateUpdaters[i] != null)
            {
                _lateUpdaters[i].LateUpdateSection();
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = _fixedUpdaters.Count - 1; i >= 0; i--)
        {
            if (_fixedUpdaters[i] != null)
            {
                _fixedUpdaters[i].FixedUpdateSection();
            }
        }
    }
    #endregion
}
