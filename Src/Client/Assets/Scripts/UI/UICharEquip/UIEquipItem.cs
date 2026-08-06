using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text level;
    public TMP_Text limitClass;
    public TMP_Text limitCategory;

    public Image background;
    public Sprite normalBG;
    public Sprite selectBG;

    private bool selected;
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            this.background.overrideSprite = selected ? selectBG : normalBG;
        }
    }

    public int index { get; set; }
    private UICharEquip owner;
    private Item item;
    bool isEquiped = false;

    void Start()
    {
        
    }



    public void SetEquipItem(int idx, Item item, UICharEquip owner, bool equiped)
    {
        this.owner = owner;
        this.index = idx;
        this.item = item;
        this.isEquiped = equiped;

        if(this.title != null) this.title.text = this.item.Define.Name;
        if (this.level != null) this.level.text = this.item.Define.Level.ToString();
        if (this.limitClass != null) this.limitClass.text = this.item.Define.LimitClass.ToString();
        if (this.limitCategory != null) this.limitCategory.text = this.item.Define.Category;
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.isEquiped)
        {
            UnEquip();
        }
        else
        {
            if (this.selected)
            {
                DoEquip();
                this.Selected = false;
            }
            else
            {
                Selected = true;
            }
                
            
        }
    }


    void DoEquip()
    {
        var msg = MessageBox.Show(string.Format("要装备{0}吗?", this.item.Define.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            //var oldEquip = EquipManager.Instance.GetEquip(item.EquipInfo.Slot);
            //if(oldEquip != null)
            //{
            //    var newmsg = MessageBox.Show(string.Format("要替换{0}吗?", this.item.Define.Name), "确认", MessageBoxType.Confirm);
            //    newmsg.OnYes = () =>
            //    {
            //        this.owner.DoEquip(this.item);
            //    };
            //}
            this.owner.DoEquip(this.item);
        };
    }


    void UnEquip()
    {
        var msg = MessageBox.Show(string.Format("要取下装备{0}吗?", this.item.Define.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            this.owner.UnEquip(this.item);
        };
    }


}
