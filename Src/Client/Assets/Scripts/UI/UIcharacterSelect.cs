using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class UIcharacterSelect : MonoBehaviour
{
    public GameObject PanelCreate;
    public GameObject PanelSelect;
    public Button CreateCancelButton;
    public Button CreateOKButton;

    public GameObject[] onclectedimages;
    private int OnclectedimageIndex = 0;
    public int Onclectedimage
    {
        get
        {
            return OnclectedimageIndex;
        }
        set
        {
            OnclectedimageIndex = value;
            UpdateClectedimage();
        }
    }


    
    public UICharacterView characterView3D;
    CharacterClass charClass;





    void Start()
    {
        
    }

    void Update()
    {
        
    }



    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass;
        characterView3D.CurrentCharacter = charClass - 1;
        this.Onclectedimage = charClass - 1;
    }

    void UpdateClectedimage()
    {
        for (int i = 0; i < onclectedimages.Length; i++)
        {
            onclectedimages[i].SetActive(i == OnclectedimageIndex);
        }
    }

}
