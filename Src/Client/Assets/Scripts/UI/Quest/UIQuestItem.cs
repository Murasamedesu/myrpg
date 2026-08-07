using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Models;

public class UIQuestItem : ListView.ListViewItem
{
    public TMP_Text title;
    public TMP_Text Type;
    public Image background;
    public Sprite normalBG;
    public Sprite selectedBG;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBG : normalBG;
    }

    public Quest quest;

    void Start()
    {
        
    }

    public void SetQuestInfo(Quest item)
    {
        this.quest = item;
        if (this.title != null) this.title.text = this.quest.Define.Name;
        if (this.Type != null) this.Type.text = "[" + this.quest.Define.Type + "]";
    }

}
