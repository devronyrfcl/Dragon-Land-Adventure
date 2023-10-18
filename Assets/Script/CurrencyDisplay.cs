using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hayyanCurrencyText;
    [SerializeField] private TextMeshProUGUI pokeCurrencyText;

    private void Start()
    {
        UpdateCurrencyTexts();
        CurrencyManager.Instance.OnHayyanCurrencyChanged += UpdateHayyanCurrencyText;
        CurrencyManager.Instance.OnPokeCurrencyChanged += UpdatePokeCurrencyText;
    }

    private void UpdateCurrencyTexts()
    {
        hayyanCurrencyText.text = CurrencyManager.Instance.CurrentHayyanCurrency.ToString();
        pokeCurrencyText.text = CurrencyManager.Instance.CurrentPokeCurrency.ToString();
    }

    private void UpdateHayyanCurrencyText(int newAmount)
    {
        hayyanCurrencyText.text = newAmount.ToString();
    }

    private void UpdatePokeCurrencyText(int newAmount)
    {
        pokeCurrencyText.text = newAmount.ToString();
    }

    private void OnDestroy()
    {
        CurrencyManager.Instance.OnHayyanCurrencyChanged -= UpdateHayyanCurrencyText;
        CurrencyManager.Instance.OnPokeCurrencyChanged -= UpdatePokeCurrencyText;
    }
}
