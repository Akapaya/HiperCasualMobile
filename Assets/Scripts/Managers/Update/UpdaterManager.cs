using System.Collections.Generic;
using UnityEngine;

public class UpdaterManager : MonoBehaviour
{
    [Header("References")]
    public static UpdaterManager Instance;

    [Header("TempData")]
    private List<IUpdater> _updaters = new(10);
    private List<ILateUpdater> _lateUpdaters = new(10);

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddIUpdaterInList(IUpdater updater)
    {
        _updaters.Add(updater);
    }

    public void RemoveIUpdaterInList(IUpdater updater)
    {
        _updaters.Remove(updater);
    }

    public void AddILateUpdaterInList(ILateUpdater updater)
    {
        _lateUpdaters.Add(updater);
    }

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
        foreach(ILateUpdater lateUpdater in _lateUpdaters)
        {
            if(lateUpdater != null)
            {
                lateUpdater.LateUpdateSection();
            }
        }
    }
}
