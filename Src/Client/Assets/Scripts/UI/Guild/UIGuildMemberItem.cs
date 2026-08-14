using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Common.Utils;

public class UIGuildMemberItem : ListView.ListViewItem
{
    public TMP_Text nickname;
    public TMP_Text @class;
    public TMP_Text level;
    public TMP_Text status;
    public TMP_Text leaderClass;
    public TMP_Text DateTime;

    public Image BG;
    public Sprite normalBG;
    public Sprite selectedBG;

    public NGuildMemberInfo Info;

    public override void onSelected(bool selected)
    {
        this.BG.overrideSprite = selected ? selectedBG : normalBG;
    }

    void Start()
    {

    }

    public void SetGuildMemberInfo(NGuildMemberInfo item)
    {
        this.Info = item;

        if (nickname != null) nickname.text = Info.Info.Name;
        if (@class != null) @class.text = Info.Info.Class.ToString();
        if (level != null) level.text = Info.Info.Level.ToString();
        if (leaderClass != null) leaderClass.text = Info.Title.ToString();
        if (DateTime != null) DateTime.text = TimeUtil.GetTime(Info.joinTime).ToShortDateString();
        if (status != null) status.text = Info.Status == 1 ? "ÔÚÏß" : TimeUtil.GetTime(Info.lastTime).ToShortDateString();
    }

}
