using UnityEngine;
using TMPro;

public class HayyanCollectedDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hayyanCollectedText;

    private int collectedHayyans;

    private void Start()
    {
        CurrencyManager.Instance.OnHayyanStoneCurrencyChanged += UpdateCollectedHayyansText;
        UpdateCollectedHayyansText(CurrencyManager.Instance.CurrentHayyanStoneCurrency);
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
        CurrencyManager.Instance.OnHayyanStoneCurrencyChanged -= UpdateCollectedHayyansText;
    }
}
