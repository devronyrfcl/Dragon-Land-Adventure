using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEnable : MonoBehaviour
{
    public int characterIndex;//0:Wheel, 1:Amy, 2:Michelle ...
    public GameObject[] shopCharacters;

    void Start()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        foreach (GameObject ch in shopCharacters)
        {
            ch.SetActive(false);
        }

        shopCharacters[characterIndex].SetActive(true);
    }

}
