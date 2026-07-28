using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Models;
using SkillBridge.Message;
using Services;


public class UIMainCity : MonoBehaviour
{
    public TMP_Text avaterName;
    public TMP_Text avaterLevel;



    void Start()
    {
        UpdateAvater();
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


}
