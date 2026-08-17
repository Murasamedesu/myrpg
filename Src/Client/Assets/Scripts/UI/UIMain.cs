using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Models;
using SkillBridge.Message;
using Services;
using System;
using Managers;


public class UIMain : MonoSingleton<UIMain>
{
    public TMP_Text avaterName;
    public TMP_Text avaterLevel;

    public UITeam TeamWindow;

    protected override void OnStart()
    {
        this.UpdateAvater();
    }


    void Update()
    {
        
    }


    public void UpdateAvater()
    {
        avaterName.text = string.Format("{0} [{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        avaterLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }


    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();
    }



    public void OnClickBag()
    {
        UIManager.Instance.ShoW<UIBag>();
    }

    public void OnClickCharEquip()
    {
        UIManager.Instance.ShoW<UICharEquip>();
    }

    public void OnClickQuestList()
    {
        UIManager.Instance.ShoW<UIQuestSystem>();
    }

    public void OnClickFriendList()
    {
        UIManager.Instance.ShoW<UIFriends>();
    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }

    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }

    public void OnClickRide()
    {
        UIManager.Instance.ShoW<UIRide>();
    }

    public void OnClickSetting()
    {
        UIManager.Instance.ShoW<UISetting>();
    }

    public void OnClickSkill()
    {

    }

}
