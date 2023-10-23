using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlayer : MonoBehaviour
{
    private GameObject player;
    //private PlayerControllerV2 playercontroller;

    public GameObject[] characterPrefabs;
    // Start is called before the first frame update

    void Awake()
    {
        int index = PlayerPrefs.GetInt("SelectedCharacter");
        Debug.Log(index);
        characterPrefabs[index].SetActive(true);
        //GameObject go = Instantiate(characterPrefabs[index], transform.position, Quaternion.identity);
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //playercontroller = player.GetComponent<PlayerControllerV2>();
        
    }

    /*public void StartGame()
    {
        playercontroller.GameStart();
    }*/

    
}
