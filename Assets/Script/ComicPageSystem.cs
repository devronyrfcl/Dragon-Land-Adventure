using UnityEngine;

public class ComicPageSystem : MonoBehaviour
{
    [SerializeField] GameObject tutorialPage;
    [SerializeField] GameObject menuButtons;
    public GameObject[] pages; // Array of comic page GameObjects

    private int currentPageIndex = 0; // Index of the current page

    private void Start()
    {
        // Check if this is the first start of the game
        // Check if this is the first start of the game
        if (!PlayerPrefs.HasKey("GameStarted") || PlayerPrefs.GetInt("GameStarted") == 0)
        {
            // First start of the game
            PlayerPrefs.SetInt("GameStarted", 1);
            PlayerPrefs.Save();

            // Enable the tutorial page
            tutorialPage.SetActive(true);
            menuButtons.SetActive(false);

            // Disable all other pages
            foreach (var page in pages)
            {
                page.SetActive(false);
            }

            // Enable the first page
            pages[0].SetActive(true);
        }
        else
        {
            // Not the first start of the game
            // Disable the tutorial page
            tutorialPage.SetActive(false);

            // Set the initial page
            if (pages.Length > 0)
            {
                SetPageActive(currentPageIndex, true);
            }
            else
            {
                Debug.LogWarning("No comic pages assigned!");
            }
        }
    
    }

    // Method to switch to the next page
    public void NextPage()
    {
        SetPageActive(currentPageIndex, false);

        currentPageIndex++;

        // Check if the current page index exceeds the number of pages
        if (currentPageIndex >= pages.Length)
        {
            Debug.Log("Reached the end of the comic!");
            tutorialPage.SetActive(false);
            menuButtons.SetActive(true);
            return;
        }

        SetPageActive(currentPageIndex, true);
    }

    // Method to switch to the previous page
    public void PreviousPage()
    {
        SetPageActive(currentPageIndex, false);

        currentPageIndex--;

        // Check if the current page index is less than 0
        if (currentPageIndex < 0)
        {
            Debug.Log("Reached the beginning of the comic!");
            tutorialPage.SetActive(false);
            menuButtons.SetActive(true);
            return;
        }

        SetPageActive(currentPageIndex, true);
    }

    // Helper method to set the active state of a page
    private void SetPageActive(int index, bool active)
    {
        if (index >= 0 && index < pages.Length)
        {
            pages[index].SetActive(active);
        }
    }

    public void StartTutorial()
    {
        //menuButtons.SetActive(false);
        tutorialPage.SetActive(true);
        foreach (var page in pages)
        {
            page.SetActive(false);
        }
        pages[0].SetActive(true);
    }
    public void Tutorial_Btn()
    {
        SetPageActive(currentPageIndex, false);
        currentPageIndex = 0;
        SetPageActive(currentPageIndex, true);
        StartTutorial();
    }

}
