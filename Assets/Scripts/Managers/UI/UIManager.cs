using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// UIManager to centralize ui changes in case of more than one script need advice for example.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public static UIManager Instance;
    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private TMP_Text _adviceNotEnoughCoinsText;

    [Header("Settins")]
    [SerializeField] private float _showAdvicesDuration;

    #region Start Methods
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    #region UI Methods
    public void ChangeTMPCoinText(int newValue)
    {
        _coinText.text = newValue.ToString();
    }

    public void AdviceNotEnoughCoins()
    {
        _adviceNotEnoughCoinsText.gameObject.SetActive(true);
        StartCoroutine(DeactiveAdvice(_adviceNotEnoughCoinsText));
    }

    private IEnumerator DeactiveAdvice(TMP_Text text)
    {
        yield return new WaitForSeconds(_showAdvicesDuration);
        text.gameObject.SetActive(false);
    }
    #endregion
}
