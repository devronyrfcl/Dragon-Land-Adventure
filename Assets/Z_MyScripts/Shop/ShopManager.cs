using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public ShopElement[] characters;

    public int characterIndex; // 0:Wheel, 1:Amy, 2:Michelle ...
    public GameObject[] shopCharacters;

    public Button buyButton;
    public Button selectCharacterButton;

    Text selectedButtonText;

    public int savedCharacterIndex;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        selectedButtonText = selectCharacterButton.GetComponentInChildren<Text>();

        // Load the isLocked data for each character
        foreach (ShopElement c in characters)
        {
            if (c.price != 0)
                c.isLocked = PlayerPrefs.GetInt(c.name, 1) == 1 ? true : false;
        }

        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        foreach (GameObject ch in shopCharacters)
        {
            ch.SetActive(false);
        }

        shopCharacters[characterIndex].SetActive(true);

        UpdateUI();
        AudioManager.Instance.StopMusic("BGM_Game");
        AudioManager.Instance.PlayMusic("BGM_Manu");
    }

    void Update()
    {
        savedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter");
    }

    public void ChangeNextCharacter()
    {
        shopCharacters[characterIndex].SetActive(false);

        characterIndex++;
        if (characterIndex == characters.Length)
            characterIndex = 0;

        shopCharacters[characterIndex].SetActive(true);

        UpdateUI();

        bool isLocked = characters[characterIndex].isLocked;
        if (isLocked)
            return;
    }

    public void ChangePreviousCharacter()
    {
        shopCharacters[characterIndex].SetActive(false);

        characterIndex--;
        if (characterIndex == -1)
            characterIndex = characters.Length - 1;

        shopCharacters[characterIndex].SetActive(true);

        UpdateUI();

        bool isLocked = characters[characterIndex].isLocked;
        if (isLocked)
            return;
    }

    public void SelectCharacter()
    {
        bool isLocked = characters[characterIndex].isLocked;
        if (isLocked)
            return;

        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        selectedButtonText.text = "Selected";
        selectCharacterButton.interactable = false;
    }

    public void UnlockWithCoins()
    {
        ShopElement c = characters[characterIndex];
        if (CurrencyManager.Instance.CurrentHayyanCurrency < c.price)
        {
            return;
        }

        CurrencyManager.Instance.RemoveHayyanCurrency(c.price);

        c.isLocked = false;
        PlayerPrefs.SetInt(c.name, 0);

        UpdateUI();
    }

    private void UpdateUI()
    {
        ShopElement c = characters[characterIndex];

        if (c.isLocked)
        {
            selectCharacterButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = c.price + "";

            if (CurrencyManager.Instance.CurrentHayyanCurrency < c.price)
                buyButton.interactable = false;
            else
                buyButton.interactable = true;
        }
        else
        {
            selectCharacterButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);

            if (PlayerPrefs.GetInt("SelectedCharacter") == characterIndex)
            {
                Debug.Log("!");
                selectedButtonText.text = "Selected";
                selectCharacterButton.interactable = false;
            }
            else
            {
                selectedButtonText.text = "Select";
                selectCharacterButton.interactable = true;
            }
        }
    }

    public void UpdateCharacterDummy()
    {
        bool isLocked = characters[characterIndex].isLocked;
        if (isLocked)
        {
            shopCharacters[characterIndex].SetActive(false);
            shopCharacters[savedCharacterIndex].SetActive(true);
            characterIndex = savedCharacterIndex;
        }
    }
}
