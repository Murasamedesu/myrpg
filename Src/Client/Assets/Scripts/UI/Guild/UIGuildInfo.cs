using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGuildInfo : MonoBehaviour
{
    public TMP_Text guildName;
    public TMP_Text guildID;
    public TMP_Text leader;
    public TMP_Text notice;
    public TMP_Text memberNumber;

    private NGuildInfo info;
    public NGuildInfo Info
    {
        get {  return info; }
        set { info = value; this.UpdateUI(); }
    }

    void UpdateUI()
    {
        if (info == null)
        {
            this.guildName.text = "无";
            this.guildID.text = "ID:0";
            this.leader.text = "会长: 无";
            this.notice.text = "";
            this.memberNumber.text = string.Format("成员数量: 0/{0}", GameDefine.GuildMaxMemberCount);
        }
        else
        {
            this.guildName.text = Info.GuildName;
            this.guildID.text = "ID:" + Info.Id;
            this.leader.text = "会长:" + Info.leaderName;
            this.notice.text = Info.Notice;
            this.memberNumber.text = string.Format("成员数量: {0}/{1}", info.memberCount, GameDefine.GuildMaxMemberCount);
        }
    }
}
