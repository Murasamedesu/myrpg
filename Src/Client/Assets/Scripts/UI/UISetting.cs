using Services;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISetting : UIWindow
{

    

    void Start()
    {
        
    }

    void Update()
    {
        
    }



    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();
    }


    public void ExitGame()
    {
        UserService.Instance.SendGameLeave(true);
    }

}
