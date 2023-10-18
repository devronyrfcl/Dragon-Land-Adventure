using UnityEngine;
using TMPro;

public class HayyanCollectedDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hayyanCollectedText;

    private int collectedHayyans;

    private void Start()
    {
        CurrencyManager.Instance.OnHayyanCurrencyChanged += UpdateCollectedHayyansText;
        UpdateCollectedHayyansText(CurrencyManager.Instance.CurrentHayyanCurrency);
    }

    private void UpdateCollectedHayyansText(int newHayyanValue)
    {
        collectedHayyans = newHayyanValue;
        UpdateHayyanCollectedText();
    }

    private void UpdateHayyanCollectedText()
    {
        hayyanCollectedText.text = collectedHayyans.ToString();
    }

    private void OnDestroy()
    {
        CurrencyManager.Instance.OnHayyanCurrencyChanged -= UpdateCollectedHayyansText;
    }
}
