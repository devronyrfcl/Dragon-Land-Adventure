using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int startingHayyanCurrency = 100;
    [SerializeField] private int startingPokeCurrency = 10;
    [SerializeField] private int startingHeartCoins = 0;
    [SerializeField] private TextMeshProUGUI hayyanCurrencyText;
    [SerializeField] private TextMeshProUGUI pokeCurrencyText;
    [SerializeField] private TextMeshProUGUI heartCoinsText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI leaderboardDistanceText;

    private int currentHayyanCurrency;
    private int currentPokeCurrency;
    private int currentHeartCoins;

    public int CurrentHayyanCurrency => currentHayyanCurrency;
    public int CurrentPokeCurrency => currentPokeCurrency;
    public int CurrentHeartCoins => currentHeartCoins;

    public event System.Action<int> OnHayyanCurrencyChanged;
    public event System.Action<int> OnPokeCurrencyChanged;
    public event System.Action<int> OnHeartCoinsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadCurrencies();
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

    public void AddHeartCoins(int amount)
    {
        currentHeartCoins += amount;
        SaveCurrencies();
        UpdateCurrencyTexts();
        OnHeartCoinsChanged?.Invoke(currentHeartCoins);
    }

    public bool RemoveHeartCoins(int amount)
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
        heartCoinsText.text = currentHeartCoins.ToString();
    }

    private void SaveCurrencies()
    {
        PlayerPrefs.SetInt("Hayyan_Currency", currentHayyanCurrency);
        PlayerPrefs.SetInt("Poke_Currency", currentPokeCurrency);
        PlayerPrefs.SetInt("Heart_Coins", currentHeartCoins);
        PlayerPrefs.Save();
    }

    private void LoadCurrencies()
    {
        currentHayyanCurrency = PlayerPrefs.GetInt("Hayyan_Currency", startingHayyanCurrency);
        currentPokeCurrency = PlayerPrefs.GetInt("Poke_Currency", startingPokeCurrency);
        currentHeartCoins = PlayerPrefs.GetInt("Heart_Coins", startingHeartCoins);
    }

    private void ShowStar()
    {
        float savedDistance = PlayerPrefs.GetFloat("DistanceTraveled", 0f);
        int totalDistance = Mathf.FloorToInt(savedDistance);

        if (distanceText != null)
        {
            distanceText.text = savedDistance.ToString("F0");
        }

        if (leaderboardDistanceText != null)
        {
            leaderboardDistanceText.text = "Distance: " + savedDistance.ToString("F0");
        }
    }
}
