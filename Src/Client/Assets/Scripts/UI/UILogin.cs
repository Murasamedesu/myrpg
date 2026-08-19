using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    public Button buttonLogin;
    public Button buttonRegister;
    public InputField username;
    public InputField password;



    void Start()
    {
        UserService.Instance.OnLogin = OnLogin;
    }

    void Update()
    {
        
    }

    void OnLogin(SkillBridge.Message.Result result, string msg)
    {
        if (result == SkillBridge.Message.Result.Success)
        {
            MessageBox.Show(string.Format("µ«¬º≥…π¶£∫{0} msg:{1}", result, msg));
            SceneManager.Instance.LoadScene("CharSelect");
            SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
        }
        else
        {
            MessageBox.Show(msg, "∑¢…˙¥ÌŒÛ", MessageBoxType.Error);
        }
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("«Î ‰»Î’À∫≈");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("«Î ‰»Î√‹¬Î");
            return;
        }

        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendLogin(this.username.text, this.password.text);

    }


}
