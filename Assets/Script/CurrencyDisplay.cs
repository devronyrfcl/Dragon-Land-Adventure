using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hayyanCurrencyText;
    [SerializeField] private TextMeshProUGUI pokeCurrencyText;
    [SerializeField] private TextMeshProUGUI heartCoinsText;

    private void Start()
    {
        UpdateCurrencyTexts();
        CurrencyManager.Instance.OnHayyanCurrencyChanged += UpdateHayyanCurrencyText;
        CurrencyManager.Instance.OnPokeCurrencyChanged += UpdatePokeCurrencyText;
        CurrencyManager.Instance.OnHeartCoinsChanged += UpdateHeartCoinsText;
    }

    private void Update()
    {
        UpdateCurrencyTexts();
    }

    private void UpdateCurrencyTexts()
    {
        hayyanCurrencyText.text = CurrencyManager.Instance.CurrentHayyanCurrency.ToString();
        pokeCurrencyText.text = CurrencyManager.Instance.CurrentPokeCurrency.ToString();
        heartCoinsText.text = CurrencyManager.Instance.CurrentHeartCoins.ToString();
    }

    private void UpdateHayyanCurrencyText(int newAmount)
    {
        hayyanCurrencyText.text = newAmount.ToString();
    }

    private void UpdatePokeCurrencyText(int newAmount)
    {
        pokeCurrencyText.text = newAmount.ToString();
    }

    private void UpdateHeartCoinsText(int newAmount)
    {
        heartCoinsText.text = newAmount.ToString();
    }

    private void OnDestroy()
    {
        CurrencyManager.Instance.OnHayyanCurrencyChanged -= UpdateHayyanCurrencyText;
        CurrencyManager.Instance.OnPokeCurrencyChanged -= UpdatePokeCurrencyText;
        CurrencyManager.Instance.OnHeartCoinsChanged -= UpdateHeartCoinsText;
    }
}
