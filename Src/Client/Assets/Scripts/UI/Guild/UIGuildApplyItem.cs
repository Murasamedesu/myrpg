using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGuildApplyItem : ListView.ListViewItem
{
    public TMP_Text nickname;
    public TMP_Text @class;
    public TMP_Text level;
    


    public NGuildApplyInfo Info;


    void Start()
    {

    }

    public void SetItemInfo(NGuildApplyInfo item)
    {
        this.Info = item;
        if(nickname != null) nickname.text = Info.Name;
        if(@class != null) @class.text = Info.Class.ToString();
        if(level != null) level.text = Info.Level.ToString();

    }

    public void OnAccept()
    {
        GuildService.Instance.SendGuildJoinApply(true, Info);
    }

    public void OnDecline()
    {
        GuildService.Instance.SendGuildJoinApply(false, Info);
    }

}
