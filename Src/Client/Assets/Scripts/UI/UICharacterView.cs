using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour
{
    public GameObject[] characters;

    private int currentCharacterIndex = 0;
    public int CurrentCharacter
    {  
        get 
        {
            return currentCharacterIndex;
        }
        set
        {
            currentCharacterIndex = value;
            UpdateCharacterView();
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void UpdateCharacterView()
    {
        for(int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == currentCharacterIndex);
        }
    }

}
