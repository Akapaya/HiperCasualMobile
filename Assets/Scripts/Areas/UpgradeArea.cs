using System;
using UnityEngine;

/// <summary>
/// The upgrade area has its own interface of areas.
/// </summary>
public class UpgradeArea : MonoBehaviour, IArea
{
    [Header("Settings")]
    [SerializeField] private AreaEnum.AreasTypes _areaType;

    [Header("Events")]
    [SerializeField] private Action<int> OnUpgradeItem;

    #region Start Methods
    private void OnEnable()
    {
        AreaManager.Instance.RegisterInDictionary(_areaType, this);
    }
    #endregion

    #region IArea Methods
    public void ActivateArea()
    {
        UpgradeManager.Instance.OpenUpgradePanel();
    }

    public void DeactivateArea()
    {
        UpgradeManager.Instance.CloseUpgradePanel();
    }

    public void RegisterObserver(Action<int> action)
    {
        OnUpgradeItem += action;
    }

    public void UnregisterObserver(Action<int> action)
    {
        OnUpgradeItem -= action;
    }
    #endregion
}
