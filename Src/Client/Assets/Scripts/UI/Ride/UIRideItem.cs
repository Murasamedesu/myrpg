using Common.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;

public class UIRideItem : ListView.ListViewItem
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

    public Item item;

    void Start()
    {

    }


    public void SetRideItem(Item item, UIRide owner, bool equiped)
    {
        this.item = item;

        if(this.title != null) this.title.text = this.item.Define.Name;
        if(this.Level != null) this.Level.text = this.item.Define.Level.ToString();
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);

    }

}
