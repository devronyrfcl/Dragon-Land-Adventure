using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int startingHayyanStoneCurrency = 2323;
    [SerializeField] private TextMeshProUGUI currencyText;

    private int currentHayyanStoneCurrency;

    public int CurrentHayyanStoneCurrency => currentHayyanStoneCurrency;

    public event System.Action<int> OnHayyanStoneCurrencyChanged;

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
        LoadHayyanStoneCurrency(); // Load saved currency on start
        UpdateCurrencyText();
    }

    public void AddHayyanStoneCurrency(int amount)
    {
        currentHayyanStoneCurrency += amount;
        SaveHayyanStoneCurrency(); // Save currency after modification
        UpdateCurrencyText();
        OnHayyanStoneCurrencyChanged?.Invoke(currentHayyanStoneCurrency);
    }

    public bool RemoveHayyanStoneCurrency(int amount)
    {
        if (currentHayyanStoneCurrency >= amount)
        {
            currentHayyanStoneCurrency -= amount;
            SaveHayyanStoneCurrency(); // Save currency after modification
            UpdateCurrencyText();
            OnHayyanStoneCurrencyChanged?.Invoke(currentHayyanStoneCurrency);
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough Hayyan Stone currency to remove.");
            return false;
        }
    }

    public void UpdateCurrencyText() // Changed to public
    {
        currencyText.text = currentHayyanStoneCurrency.ToString();
    }

    private void SaveHayyanStoneCurrency()
    {
        PlayerPrefs.SetInt("Hayyan_Stone_Currency", currentHayyanStoneCurrency);
        PlayerPrefs.Save();
    }

    public int GetHayyanStoneCurrency() // Function to get the current Hayyan Stone currency
    {
        return currentHayyanStoneCurrency;
    }

    private void LoadHayyanStoneCurrency()
    {
        if (PlayerPrefs.HasKey("Hayyan_Stone_Currency"))
        {
            currentHayyanStoneCurrency = PlayerPrefs.GetInt("Hayyan_Stone_Currency");
        }
        else
        {
            currentHayyanStoneCurrency = startingHayyanStoneCurrency;
            SaveHayyanStoneCurrency(); // Save default currency if it's the first time
        }
    }
}
