using Services;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISetting : UIWindow
{



    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
        UserService.Instance.SendGameLeave();
    }

    public void SystemConfig()
    {
        UIManager.Instance.ShoW<UISystemConfig>();
        this.Close();
    }


    public void ExitGame()
    {
        UserService.Instance.SendGameLeave(true);
    }

}
