using System;
using UnityEngine;

public class UpgradeArea : MonoBehaviour, IArea
{
    [Header("Settings")]
    [SerializeField] private AreaEnum.AreasTypes _areaType;

    [Header("Events")]
    [SerializeField] private Action<int> OnUpgradeItem;

    private void OnEnable()
    {
        AreaManager.Instance.RegisterInDictionary(_areaType, this);
    }

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
}
