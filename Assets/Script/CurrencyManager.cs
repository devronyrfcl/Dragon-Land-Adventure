using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int startingHayyanCurrency = 100;
    [SerializeField] private int startingPokeCurrency = 10;
    [SerializeField] private int startingHeartCoins = 0; // New variable
    [SerializeField] private TextMeshProUGUI hayyanCurrencyText;
    [SerializeField] private TextMeshProUGUI pokeCurrencyText;
    [SerializeField] private TextMeshProUGUI heartCoinsText; // New field
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI LeaderboarddistanceText;

    private int currentHayyanCurrency;
    private int currentPokeCurrency;
    private int currentHeartCoins; // New variable

    public int CurrentHayyanCurrency => currentHayyanCurrency;
    public int CurrentPokeCurrency => currentPokeCurrency;
    public int CurrentHeartCoins => currentHeartCoins; // New property

    public event System.Action<int> OnHayyanCurrencyChanged;
    public event System.Action<int> OnPokeCurrencyChanged;
    public event System.Action<int> OnHeartCoinsChanged; // New event

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadHayyanCurrencies();
        //LoadPokeCurrencies();
        //LoadHeartCurrencies();
        UpdateCurrencyTexts();
        ShowStar();
    }

    public void AddHayyanCurrency(int amount)
    {
        currentHayyanCurrency += amount;
        SaveCurrencies();
        UpdateCurrencyTexts();
        OnHayyanCurrencyChanged?.Invoke(currentHayyanCurrency);
    }

    public bool RemoveHayyanCurrency(int amount)
    {
        if (currentHayyanCurrency >= amount)
        {
            currentHayyanCurrency -= amount;
            SaveCurrencies();
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
        SaveCurrencies();
        UpdateCurrencyTexts();
        OnPokeCurrencyChanged?.Invoke(currentPokeCurrency);
    }

    public void RemovePoke(int amount)
    {
        currentPokeCurrency -= amount;
        SaveCurrencies();
        UpdateCurrencyTexts();
        OnPokeCurrencyChanged?.Invoke(currentPokeCurrency);
    }

    public bool RemovePokeCurrency(int amount)
    {
        if (currentPokeCurrency >= amount)
        {
            currentPokeCurrency -= amount;
            SaveCurrencies();
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

    public void AddHeartCoins(int amount) // New method
    {
        currentHeartCoins += amount;
        SaveCurrencies();
        UpdateCurrencyTexts();
        OnHeartCoinsChanged?.Invoke(currentHeartCoins);
    }

    public bool RemoveHeartCoins(int amount) // New method
    {
        if (currentHeartCoins >= amount)
        {
            currentHeartCoins -= amount;
            SaveCurrencies();
            UpdateCurrencyTexts();
            OnHeartCoinsChanged?.Invoke(currentHeartCoins);
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough Heart Coins to remove.");
            return false;
        }
    }

    private void UpdateCurrencyTexts()
    {
        hayyanCurrencyText.text = currentHayyanCurrency.ToString();
        pokeCurrencyText.text = currentPokeCurrency.ToString();
        heartCoinsText.text = currentHeartCoins.ToString(); // Display Heart Coins
    }

    private void SaveCurrencies()
    {
        PlayerPrefs.SetInt("Hayyan_Currency", currentHayyanCurrency);
        PlayerPrefs.SetInt("Poke_Currency", currentPokeCurrency);
        PlayerPrefs.SetInt("Heart_Coins", currentHeartCoins); // Save Heart Coins
        PlayerPrefs.Save();
    }

    private void LoadHayyanCurrencies()
    {
        if (PlayerPrefs.HasKey("Hayyan_Currency"))
        {
            currentHayyanCurrency = PlayerPrefs.GetInt("Hayyan_Currency");
        }
        else
        {
            currentHayyanCurrency = startingHayyanCurrency;
            SaveCurrencies();
        }

        if (PlayerPrefs.HasKey("Poke_Currency"))
        {
            currentPokeCurrency = PlayerPrefs.GetInt("Poke_Currency");
            SaveCurrencies();
        }
        else
        {
            currentPokeCurrency = startingPokeCurrency;
            SaveCurrencies();
        }

        if (PlayerPrefs.HasKey("Heart_Coins")) // Load Heart Coins
        {
            currentHeartCoins = PlayerPrefs.GetInt("Heart_Coins");
            SaveCurrencies();
        }
        else
        {
            currentHeartCoins = startingHeartCoins; // Initialize Heart Coins
            SaveCurrencies(); // Save default currencies if it's the first time
        }
    }

    /*private void LoadPokeCurrencies()
    {
        if (PlayerPrefs.HasKey("Poke_Currency"))
        {
            currentPokeCurrency = PlayerPrefs.GetInt("Poke_Currency");
            SaveCurrencies();
        }
        else
        {
            currentPokeCurrency = startingPokeCurrency;
            SaveCurrencies();
        }
    }

    private void LoadHeartCurrencies()
    {
        if (PlayerPrefs.HasKey("Heart_Coins")) // Load Heart Coins
        {
            currentHeartCoins = PlayerPrefs.GetInt("Heart_Coins");
            SaveCurrencies();
        }
        else
        {
            currentHeartCoins = startingHeartCoins; // Initialize Heart Coins
            SaveCurrencies(); // Save default currencies if it's the first time
        }
    }*/

    private void ShowStar()
    {
        // Load the saved distance from PlayerPrefs
        float savedDistance = PlayerPrefs.GetFloat("DistanceTraveled", 0f);
        int Total_Distance = Mathf.FloorToInt(savedDistance);

        // Update the TextMeshPro text with the loaded distance
        if (distanceText != null)
        {
            distanceText.text = savedDistance.ToString("F0");
        }

        if (LeaderboarddistanceText != null)
        {
            LeaderboarddistanceText.text = "Distance: " + savedDistance.ToString("F0");
        }
    }
}
