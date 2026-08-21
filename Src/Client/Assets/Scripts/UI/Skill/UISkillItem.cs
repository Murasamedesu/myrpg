using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillItem : ListView.ListViewItem
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text Level;

    public Image BG;

    public Sprite normalBG;
    public Sprite selectedBG;

    public override void onSelected(bool selected)
    {
        this.BG.overrideSprite = selected ? selectedBG : normalBG;
    }

    public SkillDefine item;

    void Start()
    {

    }


    public void SetSkillItem(SkillDefine item, UISkill owner, bool equiped)
    {
        this.item = item;

        if (this.title != null) this.title.text = this.item.Name;
        if (this.Level != null) this.Level.text = this.item.UnlockLevel.ToString();
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Icon);

    }
}
