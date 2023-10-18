using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelector : MonoBehaviour
{

    public List<GameObject> playerPrefabs;
    public List<GameObject> dummyObjects;

    private int currentIndex = 0;

    private const string SelectedPlayerIndexKey = "SelectedPlayerIndex";

    private Dictionary<string, string> sceneButtons = new Dictionary<string, string>
    {
        { "Level1", "LoadLevel1" },
        { "Level2", "LoadLevel2" },
        { "Level3", "LoadLevel3" },
        { "Level4", "LoadLevel4" },
        { "Level5", "LoadLevel5" }
    };

    private void Start()
    {
        LoadSelectedPlayerIndex();
        UpdateDummyObjects();
    }

    private void LoadSelectedPlayerIndex()
    {
        currentIndex = PlayerPrefs.GetInt(SelectedPlayerIndexKey, 0);
    }

    private void SaveSelectedPlayerIndex()
    {

        PlayerPrefs.SetInt(SelectedPlayerIndexKey, currentIndex);
    }

    private void UpdateDummyObjects()
    {
        for (int i = 0; i < dummyObjects.Count; i++)
        {
            dummyObjects[i].SetActive(i == currentIndex);
        }
    }

    public void NextPlayer()
    {
        currentIndex = (currentIndex + 1) % playerPrefabs.Count;
        UpdateDummyObjects();
    }

    public void PreviousPlayer()
    {
        currentIndex = (currentIndex - 1 + playerPrefabs.Count) % playerPrefabs.Count;
        UpdateDummyObjects();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SpawnPlayerToScene(string sceneName)
    {
        SaveSelectedPlayerIndex();
        LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneButtons.ContainsKey(scene.name))
        {
            GameObject spawnPoint = GameObject.FindWithTag("Spawnpoint");
            if (spawnPoint != null)
            {
                //int selectedPlayerIndex = PlayerPrefs.GetInt(SelectedPlayerIndexKey, 0);
                int selectedPlayerIndex = ShopManager.Instance.savedCharacterIndex;
                if (selectedPlayerIndex >= 0 && selectedPlayerIndex < playerPrefabs.Count)
                {
                    GameObject selectedPlayerPrefab = playerPrefabs[selectedPlayerIndex];
                    GameObject spawnedPlayer = Instantiate(selectedPlayerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                    spawnedPlayer.SetActive(true);
                    // You can add further logic or components to the spawned player as needed.
                }
                else
                {
                    Debug.LogError("Invalid player index selected.");
                }
            }
            else
            {
                Debug.LogError("Spawnpoint not found in scene: " + scene.name);
            }

            // Unsubscribe from the sceneLoaded event to prevent multiple subscriptions.
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
