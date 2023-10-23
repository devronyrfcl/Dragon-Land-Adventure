using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    [Header("Windows")]
    public GameObject nameWindow;
    public GameObject leaderboardWindow;

    [Header("Display Name Window")]
    public GameObject nameError;
    public InputField nameInput;
    public InputField newNameInput;
    

    [Header("Leaderboard")]
    public GameObject rowPrefab;
    public Transform rowsParent;
    public Text messageText;
    //public Text playerNameText; // Assign the Text component from the Unity Inspector

    public InputField emailInput;
    public InputField passwordInput;
    public GameObject LoginWindow;
    public GameObject UserNameWindow;
    public Text playerNameText;
    private int Total_Distance;
    

    private string playerName;

    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password is too short";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered And Logged In!";
        Login();
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = PlayFabSettings.TitleId
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    /*public void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successfully Logged In");
        messageText.text = "Logged In";
        GetTitleData();
        playerName = result.InfoResultPayload.PlayerProfile.DisplayName;

        if (string.IsNullOrEmpty(playerName))
            nameWindow.SetActive(true);
        else
            leaderboardWindow.SetActive(true);
    }*/

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successfully Logged In");
        GetTitleData();
        playerName = result.InfoResultPayload.PlayerProfile.DisplayName;

        // If the player doesn't have a Display Name, use Player ID
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = result.PlayFabId; // Use the Player ID if Display Name is not available.
        }

        // Now, you can display the playerName in your UI Text component.
        playerNameText.text = "Player Name: " + playerName;

        if (string.IsNullOrEmpty(playerName))
            nameWindow.SetActive(true);
        else
            leaderboardWindow.SetActive(true);
    }



    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        Debug.Log("Password reset email sent successfully");
        messageText.text = "Password reset email sent. Please check your email.";
    }

    void Start()
    {
        Login();
        float savedDistance = PlayerPrefs.GetFloat("DistanceTraveled", 0f);
        Total_Distance = Mathf.FloorToInt(savedDistance);
        SendToLeaderBoardFuntion();
        //SendLeaderboard(Total_Distance);
        
    }


    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successfully Logged In");
        messageText.text = "Logged In";
        string name = null;
        GetTitleData();
        playerName = result.InfoResultPayload.PlayerProfile.DisplayName;
        //playerNameText.text = "Player Name: " + playerName;


        if (string.IsNullOrEmpty(playerName))
            nameWindow.SetActive(true);
        else
            //leaderboardWindow.SetActive(true);
            LoginWindow.SetActive(false);
    }

    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);

    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated Display Name");
        playerName = result.DisplayName;
        //LoginWindow.SetActive(false);
        UserNameWindow.SetActive(false);
        playerNameText.text = playerName;
        

        
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error");
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int star)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Star Leaderboard",
                    Value = star
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfully updated leaderboard");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Star Leaderboard",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
        //ShowNameOnLeaderboard();
    }

    public void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
        }
    }
    /*public void ShowNameOnLeaderboard(GetLeaderboardResult result)
    {
        Leaderboard_Name_Show_On_Top.text = item.DisplayName;
    }*/

    void GetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleDataReceived, OnError);
    }

    void OnTitleDataReceived(GetTitleDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("Message"))
        {
            Debug.Log("No Message");
            return;
        }
        //messageText.text = result.Data["Message"];
    }

    public void ChangeNameButtonOnClick()
    {
        string newDisplayName = newNameInput.text;

        if (string.IsNullOrEmpty(newDisplayName))
        {
            // Handle error: Display name cannot be empty.
            // You can show an error message or take appropriate action here.
            return;
        }

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newDisplayName,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    private void SendToLeaderBoardFuntion()
    {
        SendLeaderboard(Total_Distance);
        Debug.Log("Done...Done..........");
    }
    
    
}
