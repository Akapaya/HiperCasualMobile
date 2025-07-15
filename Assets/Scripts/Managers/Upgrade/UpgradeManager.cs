using UnityEngine;
using PrimeTween;
using System;
/// <summary>
/// Manager to handle possible upgrades and purchases.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    [Header("References")]
    public static UpgradeManager Instance;
    public UpgradeSO[] Upgrades = new UpgradeSO[10];
    public UpgradePanel[] UpgradePanels = new UpgradePanel[10];

    [Header("Settings")]
    [SerializeField] private RectTransform _upgradePanel;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private Vector2 _offscreenOffset = new Vector2(1500f, 0f);
    [SerializeField] private Ease _easeAnimation;

    [Header("Temp Data")]
    Tween _tween;

    [Header("Events")]
    public Action<UpgradeSO> OnPurchasedUpgrade;

    #region Start Methods
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    #region Upgrades Methods
    /// <summary>
    /// Choose randomly 3 Upgrades to show in upgrade panels
    /// </summary>
    public void OpenUpgradePanel()
    {
        _upgradePanel.anchoredPosition = _offscreenOffset;
        _upgradePanel.gameObject.SetActive(true);

        _tween = Tween.UIAnchoredPosition(_upgradePanel, Vector2.zero, _duration, _easeAnimation);

        foreach (UpgradePanel upgradePanel in UpgradePanels)
        {
            upgradePanel.SetUpgradePanelData(Upgrades[UnityEngine.Random.Range(0, Upgrades.Length - 1)]);
        }
    }

    /// <summary>
    /// Close Upgrade Panel
    /// </summary>
    public void CloseUpgradePanel()
    {
        _tween = Tween.UIAnchoredPosition(_upgradePanel, _offscreenOffset, _duration, _easeAnimation)
            .OnComplete(() => _upgradePanel.gameObject.SetActive(false));
    }

    /// <summary>
    /// Invoke event in case of buy a upgrade
    /// </summary>
    /// <param name="upgradeSO">Data of upgrade Purchased</param>
    public void PurchaseUpgrade(UpgradeSO upgradeSO)
    {
        OnPurchasedUpgrade?.Invoke(upgradeSO);
    }
    #endregion
}
