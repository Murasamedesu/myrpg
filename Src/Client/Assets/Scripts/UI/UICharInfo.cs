using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour
{
    public TMP_Text charName;
    public TMP_Text charClass;
    public Image profileTitel;
    public Image highlight;
    public SkillBridge.Message.NCharacterInfo info;

    public Sprite ArcherArcherAvatar;
    public Sprite WarriorAvatar;
    public Sprite WizardAvatar;


    public bool Selected
    {
        get { return highlight.IsActive(); }
        set
        {
            highlight.gameObject.SetActive(value);
        }
    }





    void Start()
    {
        
        if(info != null)
        {
            this.charClass.text = this.info.Class.ToString();
            this.charName.text = this.info.Name;
            UpdateProfile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UpdateProfile()
    {
        if (info != null)
        {
            if (info.Class == SkillBridge.Message.CharacterClass.Archer)
            {
                profileTitel.sprite = ArcherArcherAvatar;
            }
            else if(info.Class == SkillBridge.Message.CharacterClass.Warrior)
            {
                profileTitel.sprite = WarriorAvatar;
            }
            else if(info.Class == SkillBridge.Message.CharacterClass.Wizard)
            {
                profileTitel.sprite = WizardAvatar;
            }

        }

    }

}
