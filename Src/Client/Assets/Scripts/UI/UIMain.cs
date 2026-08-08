using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Models;
using SkillBridge.Message;
using Services;
using System;


public class UIMain : MonoSingleton<UIMain>
{
    public TMP_Text avaterName;
    public TMP_Text avaterLevel;



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


    public void OnClickTest()
    {
        UITest test = UIManager.Instance.ShoW<UITest>();
        test.SetTitle("这是一个测试UI");
        test.OnClose += Test_OnClose;

    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("TEST:: 点击了测试UI对话框的" + result, "对话框响应结果", MessageBoxType.Information);

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
}
