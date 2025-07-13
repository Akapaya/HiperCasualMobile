using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public static UIManager Instance;
    [SerializeField] private TMP_Text _coinText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ChangeTMPCoinText(int newValue)
    {
        _coinText.text = newValue.ToString();
    }
}
