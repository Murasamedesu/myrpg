using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem
{
    public TMP_Text nickname;
    public TMP_Text ID;
    public TMP_Text members;
    public TMP_Text leader;

    public Image BG;
    public Sprite normalBG;
    public Sprite selectedBG;

    public NGuildInfo Info;

    public override void onSelected(bool selected)
    {
        this.BG.overrideSprite = selected ? selectedBG : normalBG;
    }

    void Start()
    {

    }

    public void SetGuildInfo(NGuildInfo item)
    {
        this.Info = item;

        if (nickname != null) nickname.text = Info.GuildName;
        if (ID != null) ID.text = Info.Id.ToString();
        if (members != null) members.text = string.Format("{0}/{1}", Info.memberCount, GameDefine.GuildMaxMemberCount);
        if (leader != null) leader.text = Info.leaderName;
        
        
    }

}
