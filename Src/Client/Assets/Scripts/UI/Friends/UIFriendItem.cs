using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendItem : ListView.ListViewItem
{
    public TMP_Text nickname;
    public TMP_Text @class;
    public TMP_Text level;
    public TMP_Text status;

    public Image BG;
    public Sprite normalBG;
    public Sprite selectedBG;

    public NFriendInfo Info;

    public override void onSelected(bool selected)
    {
        this.BG.overrideSprite = selected ? selectedBG : normalBG;
    }

    void Start()
    {
        
    }

    public void SetFriendInfo(NFriendInfo item)
    {
        this.Info = item;

        if(nickname != null) nickname.text = Info.friendInfo.Name;
        if(@class != null) @class.text = Info.friendInfo.Class.ToString();
        if(level != null) level.text = Info.friendInfo.Level.ToString();
        if (status != null) status.text = Info.Status == 1 ? "‘⁄œﬂ" : "¿Îœﬂ";
    }



}
