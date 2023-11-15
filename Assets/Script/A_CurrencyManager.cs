using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class A_CurrencyManager : MonoBehaviour
{
    public static A_CurrencyManager Instance { get; private set; }

    [SerializeField] private int startingCoinCurrency = 100;
    [SerializeField] private int startingDiamondCurrency = 10;
    [SerializeField] private TextMeshProUGUI coinCurrencyText;
    [SerializeField] private TextMeshProUGUI diamondCurrencyText;

    private int currentCoinCurrency;
    private int currentDiamondCurrency;

    public int CurrentCoinCurrency => currentCoinCurrency;
    public int CurrentDiamondCurrency => currentDiamondCurrency;

    public event System.Action<int> OnCoinCurrencyChanged;
    public event System.Action<int> OnDiamondCurrencyChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the CurrencyManager across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        LoadCurrencies(); // Load saved currencies on start
        UpdateCurrencyTexts();
    }

    public void AddCoinCurrency(int amount)
    {
        currentCoinCurrency += amount;
        SaveCurrencies(); // Save currencies after modification
        UpdateCurrencyTexts();
        OnCoinCurrencyChanged?.Invoke(currentCoinCurrency);
    }

    public bool RemoveCoinCurrency(int amount)
    {
        if (currentCoinCurrency >= amount)
        {
            currentCoinCurrency -= amount;
            SaveCurrencies(); // Save currencies after modification
            UpdateCurrencyTexts();
            OnCoinCurrencyChanged?.Invoke(currentCoinCurrency);
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough Coins to remove.");
            return false;
        }
    }

    public void AddDiamondCurrency(int amount)
    {
        currentDiamondCurrency += amount;
        SaveCurrencies(); // Save currencies after modification
        UpdateCurrencyTexts();
        OnDiamondCurrencyChanged?.Invoke(currentDiamondCurrency);
    }

    public void RemoveDiamond(int amount)
    {
        currentDiamondCurrency -= amount;
        SaveCurrencies(); // Save currencies after modification
        UpdateCurrencyTexts();
        OnDiamondCurrencyChanged?.Invoke(currentDiamondCurrency);
    }

    public bool RemoveDiamondCurrency(int amount)
    {
        if (currentDiamondCurrency >= amount)
        {
            currentDiamondCurrency -= amount;
            SaveCurrencies(); // Save currencies after modification
            UpdateCurrencyTexts();
            OnDiamondCurrencyChanged?.Invoke(currentDiamondCurrency);
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough Diamonds to remove.");
            return false;
        }
    }

    private void UpdateCurrencyTexts()
    {
        coinCurrencyText.text = currentCoinCurrency.ToString();
        diamondCurrencyText.text = currentDiamondCurrency.ToString();
    }

    private void SaveCurrencies()
    {
        PlayerPrefs.SetInt("Coin_Currency", currentCoinCurrency);
        PlayerPrefs.SetInt("Diamond_Currency", currentDiamondCurrency);
        PlayerPrefs.Save();
    }

    private void LoadCurrencies()
    {
        if (PlayerPrefs.HasKey("Coin_Currency"))
        {
            currentCoinCurrency = PlayerPrefs.GetInt("Coin_Currency");
        }
        else
        {
            currentCoinCurrency = startingCoinCurrency;
            SaveCurrencies(); // Save default currencies if it's the first time
        }

        if (PlayerPrefs.HasKey("Diamond_Currency"))
        {
            currentDiamondCurrency = PlayerPrefs.GetInt("Diamond_Currency");
        }
        else
        {
            currentDiamondCurrency = startingDiamondCurrency;
            SaveCurrencies(); // Save default currencies if it's the first time
        }
    }


}
