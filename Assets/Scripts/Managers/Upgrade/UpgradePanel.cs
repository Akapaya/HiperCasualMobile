using TMPro;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _descriptionText;

    [Header("Temp Data")]
    [SerializeField] private UpgradeSO _upgradeSO;

    public void SetUpgradePanelData(UpgradeSO upgradeSO)
    {
        _upgradeSO = upgradeSO;
        _descriptionText.text = _upgradeSO.Description;
        _costText.text = _upgradeSO.Cost.ToString();
    }

    public void PurchaseUpgrade()
    {
        UpgradeManager.Instance.PurchaseUpgrade(_upgradeSO);
    }
}
