using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITeamItem : ListView.ListViewItem
{
    public TMP_Text nickname;
    public TMP_Text level;
    public Image classIcon;
    public Image leaderIcon;

    public Image background;

    public int idx;

    public override void onSelected(bool selected)
    {
        this.background.enabled = selected ? true : false;
    }

    public NCharacterInfo Info;

    void Start()
    {
        this.background.enabled = false;
    }

    public void SetMemberInfo(int idx, NCharacterInfo item, bool isLeader)
    {
        this.idx = idx;
        this.Info = item;


        if (this.nickname != null) this.nickname.text = this.Info.Name;
        if (this.level != null) this.level.text = "Lv." + this.Info.Level.ToString();
        if (this.classIcon != null) this.classIcon.overrideSprite = SpriteManager.Instance.classIcons[(int)this.Info.Class - 1];
        if (this.leaderIcon != null) this.leaderIcon.gameObject.SetActive(isLeader);
    }

}
