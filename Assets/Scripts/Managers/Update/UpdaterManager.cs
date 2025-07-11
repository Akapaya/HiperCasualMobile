using System.Collections.Generic;
using UnityEngine;

public class UpdaterManager : MonoBehaviour
{
    [Header("References")]
    public static UpdaterManager Instance;

    [Header("TempData")]
    private List<IUpdater> _updaters = new(10);
    private List<ILateUpdater> _lateUpdaters = new(10);

    void Start()
    {
        Instance = this;
    }

    public void AddIUpdaterInList(IUpdater updater)
    {
        _updaters.Add(updater);
    }

    public void AddILateUpdaterInList(ILateUpdater updater)
    {
        _lateUpdaters.Add(updater);
    }

    void Update()
    {
        foreach (IUpdater updater in _updaters)
        {
            if(updater != null)
            {
                updater.UpdateSection();
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
