using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{

    public static ShopManager Instance;


    public ShopElement[] characters;

    public int characterIndex;//0:Wheel, 1:Amy, 2:Michelle ...
    public GameObject[] shopCharacters;

    //public GameObject rewardMenu;
    //public TextMeshProUGUI rewardText;
    //public GameObject mainMenu;

    public Button buyButton;

    public Button selectCharacterButton;

    Text selectedButtonText;

    public int savedCharacterIndex;


    

    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {

        PlayerPrefs.SetInt("InGameCurrency", 1000);

        selectedButtonText = selectCharacterButton.GetComponentInChildren<Text>();

        //Load the isLocked data for each character
        foreach(ShopElement c in characters)
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

        //PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
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

        //PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
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
        if (PlayerPrefs.GetInt("Hayyan_Currency") < c.price)
        {
            return;
        }
        /*int newGems = PlayerPrefs.GetInt("Coin_Currency") - characters[characterIndex].price;
        PlayerPrefs.SetInt("Coin_Currency", newGems);*/

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

            if (PlayerPrefs.GetInt("Hayyan_Currency", 0) < c.price)
                buyButton.interactable = false;
            else
                buyButton.interactable = true;
        }
        else
        {
            selectCharacterButton.gameObject.SetActive(true);         
            buyButton.gameObject.SetActive(false);

            if(PlayerPrefs.GetInt("SelectedCharacter") == characterIndex)
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
