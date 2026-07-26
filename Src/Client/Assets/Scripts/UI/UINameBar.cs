using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Entities;

public class UINameBar : MonoBehaviour
{
    public TMP_Text avaterName;
    public Image profileimage;

    public Sprite ArcherArcherAvatar;
    public Sprite WarriorAvatar;
    public Sprite WizardAvatar;

    public Character character;

    public string CurrentprofieData { get; set; }


    // Start is called before the first frame update
    void Start()
    {
       if(character != null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();
    }


    void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = this.character.Name + " Lv." + this.character.Info.Level;
            if (name != this.avaterName.text)
            {
                this.avaterName.text = name;
            }



            if (character.Info.Class == SkillBridge.Message.CharacterClass.Archer && ArcherArcherAvatar != profileimage.sprite)
            {
                profileimage.sprite = ArcherArcherAvatar;
            }
            else if (character.Info.Class == SkillBridge.Message.CharacterClass.Warrior && WarriorAvatar != profileimage.sprite)
            {
                profileimage.sprite = WarriorAvatar;
            }
            else if (character.Info.Class == SkillBridge.Message.CharacterClass.Wizard && WizardAvatar != profileimage.sprite)
            {
                profileimage.sprite = WizardAvatar;
            }
        }
    }



    
        

        
}
