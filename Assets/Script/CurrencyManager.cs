using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int startingHayyanCurrency = 100;
    [SerializeField] private int startingPokeCurrency = 10;
    [SerializeField] private TextMeshProUGUI hayyanCurrencyText;
    [SerializeField] private TextMeshProUGUI pokeCurrencyText;

    private int currentHayyanCurrency;
    private int currentPokeCurrency;

    public int CurrentHayyanCurrency => currentHayyanCurrency;
    public int CurrentPokeCurrency => currentPokeCurrency;

    public event System.Action<int> OnHayyanCurrencyChanged;
    public event System.Action<int> OnPokeCurrencyChanged;

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

    public void AddHayyanCurrency(int amount)
    {
        currentHayyanCurrency += amount;
        SaveCurrencies(); // Save currencies after modification
        UpdateCurrencyTexts();
        OnHayyanCurrencyChanged?.Invoke(currentHayyanCurrency);
    }

    public bool RemoveHayyanCurrency(int amount)
    {
        if (currentHayyanCurrency >= amount)
        {
            currentHayyanCurrency -= amount;
            SaveCurrencies(); // Save currencies after modification
            UpdateCurrencyTexts();
            OnHayyanCurrencyChanged?.Invoke(currentHayyanCurrency);
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough Hayyans to remove.");
            return false;
        }
    }

    public void AddPokeCurrency(int amount)
    {
        currentPokeCurrency += amount;
        SaveCurrencies(); // Save currencies after modification
        UpdateCurrencyTexts();
        OnPokeCurrencyChanged?.Invoke(currentPokeCurrency);
    }

    public void RemovePoke(int amount)
    {
        currentPokeCurrency -= amount;
        SaveCurrencies(); // Save currencies after modification
        UpdateCurrencyTexts();
        OnPokeCurrencyChanged?.Invoke(currentPokeCurrency);
    }

    public bool RemovePokeCurrency(int amount)
    {
        if (currentPokeCurrency >= amount)
        {
            currentPokeCurrency -= amount;
            SaveCurrencies(); // Save currencies after modification
            UpdateCurrencyTexts();
            OnPokeCurrencyChanged?.Invoke(currentPokeCurrency);
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough Pokes to remove.");
            return false;
        }
    }

    private void UpdateCurrencyTexts()
    {
        hayyanCurrencyText.text = currentHayyanCurrency.ToString();
        pokeCurrencyText.text = currentPokeCurrency.ToString();
    }

    private void SaveCurrencies()
    {
        PlayerPrefs.SetInt("Hayyan_Currency", currentHayyanCurrency);
        PlayerPrefs.SetInt("Poke_Currency", currentPokeCurrency);
        PlayerPrefs.Save();
    }

    private void LoadCurrencies()
    {
        if (PlayerPrefs.HasKey("Hayyan_Currency"))
        {
            currentHayyanCurrency = PlayerPrefs.GetInt("Hayyan_Currency");
        }
        else
        {
            currentHayyanCurrency = startingHayyanCurrency;
            SaveCurrencies(); // Save default currencies if it's the first time
        }

        if (PlayerPrefs.HasKey("Poke_Currency"))
        {
            currentPokeCurrency = PlayerPrefs.GetInt("Poke_Currency");
        }
        else
        {
            currentPokeCurrency = startingPokeCurrency;
            SaveCurrencies(); // Save default currencies if it's the first time
        }
    }
}
