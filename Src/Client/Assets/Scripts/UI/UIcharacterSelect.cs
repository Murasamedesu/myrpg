using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using Services;
using Models;

public class UIcharacterSelect : MonoBehaviour
{
    public GameObject PanelCreate;
    public GameObject PanelSelect;
    public Button CreateCancelButton;
    public Button CreateOKButton;

    public GameObject[] onclectedimages;
    public TMP_Text[] describe;
    public TMP_InputField charName;

    public UICharacterView characterView3D;
    CharacterClass charClass;

    public Transform uiCharList;
    public GameObject uiCharInfo;
    public List<GameObject> uiChars = new List<GameObject>();

    public int selectCharacterIdx = -1;


    void Start()
    {
        InitCharacterSelect(true);
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;
    }
    



    public void InitCharacterSelect(bool init)
    {
        PanelCreate.SetActive(false);
        PanelSelect.SetActive(true);

        if(init)
        {
            foreach (var old in uiChars)
            {
                Destroy(old);
            }
            uiChars.Clear();

            for(int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
            {
                GameObject Item = Instantiate(uiCharInfo, this.uiCharList);
                UICharInfo chrinfo = Item.GetComponent<UICharInfo>();
                chrinfo.info = User.Instance.Info.Player.Characters[i];

                Button button = Item.GetComponent<Button>();
                int idx = i;
                button.onClick.AddListener(() =>
                {
                    OnSelectCharacter(idx);
                });

                uiChars.Add(Item);
                Item.SetActive(true);
            }
        }


        Debug.Log($"[InitCharacterSelect] 开始初始化，当前角色数量: {User.Instance.Info.Player.Characters.Count}");


    }

    public void InitCharacterCreate()
    {
        PanelCreate.SetActive(true);
        PanelSelect.SetActive(false);
        OnSelectClass(1);
    }

    public void OnSelectCharacter(int idx)
    {
        this.selectCharacterIdx = idx;
        var cha = User.Instance.Info.Player.Characters[idx];
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);

        characterView3D.CurrentCharacter = ((int)cha.Class - 1);
        for(int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
            ci.Selected = idx == i;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);

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
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }



    void OnCharacterCreate(SkillBridge.Message.Result result, string msg)
    {
        if(result == Result.Success)
        {
            InitCharacterSelect(true);
        }
        else
            MessageBox.Show(msg, "错误", MessageBoxType.Error);
        
    }



    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(this.charName.text))
        {
            MessageBox.Show("请输入角色名");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendCharacterCreate(this.charName.text, this.charClass);
    }

    public void OnClickPlay()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        if (selectCharacterIdx >= 0)
        {
            UserService.Instance.SendGameEnter(selectCharacterIdx);
        }
    }



}
