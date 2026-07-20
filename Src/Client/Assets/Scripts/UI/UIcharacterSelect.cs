using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using Services;

public class UIcharacterSelect : MonoBehaviour
{
    public GameObject PanelCreate;
    public GameObject PanelSelect;
    public Button CreateCancelButton;
    public Button CreateOKButton;

    public GameObject[] onclectedimages;
    public TMP_Text[] describe;


    
    public UICharacterView characterView3D;
    CharacterClass charClass;



  

    void Start()
    {
        InitCharacterSelect(true);
        DataManager.Instance.Load();
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;
    }


    public void InitCharacterSelect(bool init)
    {
        PanelCreate.SetActive(false);
        PanelSelect.SetActive(true);

        if(init)
        {

        }


    }





    void Update()
    {
        
    }



    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass;
        characterView3D.CurrentCharacter = charClass - 1;
        
        for (int i = 0; i < onclectedimages.Length; i++)
        {
            onclectedimages[i].SetActive(i == charClass - 1);
            describe[i].text = DataManager.Instance.Characters[i + 1].Description;
        }
        
    }




    void OnCharacterCreate(SkillBridge.Message.Result result, string msg)
    {

    }


}
